using System.Net.Http.Headers;

namespace HyRest.Identity;

/// <summary>
/// Handles setting the Authentication Header when dependancy injection is not used.
/// </summary>
public sealed class BearerTokenHandler : DelegatingHandler
{
    private readonly IHylandAuthClient _authClient;
    private readonly HylandClientOptions _options;
    public BearerTokenHandler(HylandBasicAuthClient authClient, IAuthenticationCredentials creds, HylandClientOptions options)
        : base()
    {
        _authClient = authClient.WithCredentials(creds);
        _options = options;
    }
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _authClient.GetAccessToken();
        if (token != null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        if (_options.UseQueryMetering)
            request.Headers.Add("Hyland-License-Type", "QueryMetering");

        return base.SendAsync(request, cancellationToken);
    }
}
