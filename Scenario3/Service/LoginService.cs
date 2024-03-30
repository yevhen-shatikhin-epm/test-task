using System.Net;
using RestSharp;
using Scenario3.Models;

namespace Scenario3.Service;

public class LoginService
{
    private readonly RestClient _client;

    public LoginService(RestClient client)
    {
        _client = client;
    }

    public void Login(string username, string password)
    {
        // some implementation
    }

    public IEnumerable<LoginFail> GetLoginFailTotal(string? username = null, int? failCount = null, int? fetchLimit = null)
    {
        var response = _client.GetJson<IEnumerable<LoginFail>>(
            "loginfailtotal?user_name={username}?fail_count={failCount}?fetch_limit={fetchLimit}",
            new
            {
                username,
                failCount,
                fetchLimit
            }
        );

        return response!;
    }

    public HttpStatusCode ResetLoginFailTotal(string username)
    {
        var response = _client.PutJson("resetloginfailtotal", new { username });

        return response;
    }
}