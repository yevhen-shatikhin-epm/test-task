using System.Net;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using Scenario3.Service;

namespace Scenario3;

[TestFixture]
public class LoginFailTests
{
    public RestClient Client;

    public LoginService LoginService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Client = new RestClient(new RestClientOptions(@"https://api.login.com"));
        LoginService = new LoginService(Client);
    }

    [Test]
    public void LoginFailTotalEndpointShouldReturnFailCountOnlyForTheSpecifiedUser()
    {
        var username = "username";

        LoginService.ResetLoginFailTotal(username);
        LoginService.Login(username, "");

        var loginFails = LoginService.GetLoginFailTotal(username).ToList();

        loginFails.Should().HaveCount(1);

        var loginFail = loginFails.First();
        loginFail.Username.Should().Be(username);
        loginFail.FailCount.Should().Be(1);
    }

    [Test]
    public void LoginFailTotalEndpointShouldReturnFailCountForAllUsersIfNotSpecified()
    {
        var usernames = new[]
        {
            "username1",
            "username2"
        };

        foreach (var username in usernames)
        {
            LoginService.ResetLoginFailTotal(username);
            LoginService.Login(username, "");
        }

        var loginFails = LoginService.GetLoginFailTotal().ToList();

        loginFails.Select(lf => lf.Username).Should().Contain(usernames);
    }

    [Test]
    public void LoginFailTotalEndpointShouldOnlyReturnUsersWithNumberOfFailsGreaterThanSpecified()
    {
        var logins = new[]
        {
            new
            {
                Username = "username1",
                Count = 3
            },
            new
            {
                Username = "username1",
                Count = 2
            },
            new
            {
                Username = "username1",
                Count = 1
            },
        };

        foreach (var login in logins)
        {
            LoginService.ResetLoginFailTotal(login.Username);
            for (int i = 0; i < login.Count; i++)
            {
                LoginService.Login(login.Username, "");
            }
        }

        foreach (var login in logins)
        {
            var loginFails = LoginService.GetLoginFailTotal(failCount: login.Count).ToList();

            var expectedUsers = logins.Where(l => l.Count > login.Count).Select(l => l.Username);
            var notExpectedUsers = logins.Where(l => l.Count <= login.Count).Select(l => l.Username);

            var actualUsers = loginFails.Select(lf => lf.Username).ToList();

            actualUsers.Should().Contain(expectedUsers);
            actualUsers.Should().NotContain(notExpectedUsers);
        }
    }

    [Test]
    public void LoginFailTotalEndpointShouldReturnLimitedNumberOfResults()
    {
        var usernames = new[]
        {
            "username1",
            "username2",
            "username3"
        };

        foreach (var username in usernames)
        {
            LoginService.ResetLoginFailTotal(username);
            LoginService.Login(username, "");
        }

        // should return all
        var loginFails = LoginService.GetLoginFailTotal().ToList();
        loginFails.Select(lf => lf.Username).Should().Contain(usernames);

        // should be limited
        loginFails = LoginService.GetLoginFailTotal(fetchLimit: 2).ToList();
        loginFails.Should().HaveCount(2);

        loginFails = LoginService.GetLoginFailTotal(fetchLimit: 1).ToList();
        loginFails.Should().HaveCount(1);
    }

    [Test]
    public void ResetLoginFailTotalEndpointShouldResetTotalFailCountForValidUser()
    {
        var expectedResponseCode = HttpStatusCode.OK;
        var username = "username";

        LoginService.Login(username, "");

        LoginService.GetLoginFailTotal(username).First().FailCount.Should().Be(1);

        // verify that 1 attempt is reset
        var actualResponseCode = LoginService.ResetLoginFailTotal(username);

        actualResponseCode.Should().Be(expectedResponseCode);

        LoginService.GetLoginFailTotal(username).First().FailCount.Should().Be(0);

        // verify that resetting 0 attempts results in OK
        actualResponseCode = LoginService.ResetLoginFailTotal(username);

        actualResponseCode.Should().Be(expectedResponseCode);

        LoginService.GetLoginFailTotal(username).First().FailCount.Should().Be(0);
    }

    [Test]
    public void ResetLoginFailTotalEndpointsShouldReturnNotFoundWhenTryingToResetForInvalidUser()
    {
        var expectedResponseCode = HttpStatusCode.NotFound;
        var invalidUsername = "404";

        var actualResponseCode = LoginService.ResetLoginFailTotal(invalidUsername);

        actualResponseCode.Should().Be(expectedResponseCode);
    }
}