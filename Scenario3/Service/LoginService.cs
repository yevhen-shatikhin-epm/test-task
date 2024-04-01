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
        var request = new RestRequest("loginfailtotal", Method.Get);

        if (username != null) request.AddParameter("user_name", username);
        if (failCount != null) request.AddParameter("fail_count", (int)failCount);
        if (fetchLimit != null) request.AddParameter("fetch_limit", (int)fetchLimit);

        var response = _client.Get<IEnumerable<LoginFail>>(request);

        return response!;
    }

    public HttpStatusCode ResetLoginFailTotal(string username)
    {
        var response = _client.PutJson("resetloginfailtotal", new { username });

        return response;
    }
}