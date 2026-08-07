using Microsoft.Extensions.Options;
using HyRest.Session;

namespace HyRest;

/// <summary>
/// Session Cookie Handler for adding the 'Set-Cookie' session Id. Also provides a SessionCookie object for the SessionAPI
/// </summary>
public sealed class SessionCookieClientHandler : HttpClientHandler
{
    private IHylandClientOptions _options { get; set; } = new HylandClientOptions();
    public SessionCookieClientHandler(IOptions<HylandOpenIdClientOptionsBuilder> options) : base()
    {
        this.UseCookies = true;
        this.AllowAutoRedirect = true;
        this.CookieContainer = new System.Net.CookieContainer();
        options.Value.OptionsAction(_options);
    }
    internal SessionCookieClientHandler(IHylandClientOptions options) : base()
    {
        this.UseCookies = true;
        this.AllowAutoRedirect = true;
        this.CookieContainer = new System.Net.CookieContainer();
        _options = options;
    }
    public SessionCookie? GetSessionCookie()
    {
        var cookie = CookieContainer
            .GetCookies(new Uri(_options.ApiBaseUrl))
            .FirstOrDefault(c => c.Name == "Cookie.Session.OnBase.Hyland");
        if (cookie != null)
            return SessionCookie.Create(cookie);
        return null;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cookie = GetSessionCookie();
        if(cookie != null)
            request.Headers.Add("Set-Cookie", cookie.SessionId);
        if (_options.UseQueryMetering)
            request.Headers.Add("Hyland-License-Type", "QueryMetering");
        return base.SendAsync(request, cancellationToken);
    }
}
