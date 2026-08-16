using HyRest.Session;
using System.Net;

namespace HyRest;

public class HylandApiClient : IHylandApiClient
{
    private readonly HttpClient _httpClient;
    private CookieContainer _cookieContainer;
    private readonly IOnBaseSessionAPI _api;
    public HylandApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _api = IHylandRestAPI.Get<IOnBaseSessionAPI>(_httpClient, HylandClientFactory.Settings);
    }
    public HttpClient HttpClient => _httpClient;
    public CookieContainer CookieContainer => _cookieContainer;
    public SessionCookie? SessionCookie => GetSessionCookie();
    public bool IsActive => SessionCookie != null ? !SessionCookie.Expired : false;
    public async Task RefreshSessionAsync()
    {
        if (SessionCookie == null || SessionCookie.Expired)
            await _api.InitiateSessionAsync();
        else
            await _api.HeartbeatAsync();
    }
    public IHylandApiClient WithCookieContainer(CookieContainer CookieContainer)
    {
        _cookieContainer = CookieContainer;
        return this;
    }
    internal SessionCookie? GetSessionCookie()
    {
        if (CookieContainer != null)
        {
            var cookie = CookieContainer
            .GetCookies(HttpClient.BaseAddress)
            .FirstOrDefault(c => c.Name == "Cookie.Session.OnBase.Hyland");
            if (cookie != null)
                return SessionCookie.Create(cookie);            
        }

        return null;
    }

}

