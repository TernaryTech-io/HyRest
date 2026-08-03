using Microsoft.Extensions.Logging;


namespace HyRest.Session;

public sealed partial class OnBaseSession : OnBaseModule<IOnBaseSessionAPI>, IOnBaseSession
{
    internal OnBaseSession(IOnBaseApp app) : base(app)
    {
        
    }
    /// <summary>
    /// Initiates the OnBase session and captures the session cookie by calling
    /// a lightweight endpoint. Call this right after creating the client to ensure
    /// the session is established before any document operations begin.
    /// </summary>
    public Task InitiateAsync() => Run(Api<IOnBaseSessionAPI>().InitiateSessionAsync());

    /// <summary>
    /// Initiates the OnBase session and captures the session cookie by calling
    /// a lightweight endpoint. Call this right after creating the client to ensure
    /// the session is established before any document operations begin.
    /// </summary>
    public void Initiate() => InitiateAsync().Wait();

    /// <summary>
    /// Refreshes the session cookie, extending the session lifetime by 5 minutes.
    /// Call this every 4–5 minutes while idle to prevent the OnBase session from expiring.
    /// </summary>
    public Task HeartbeatAsync() => Run(Api<IOnBaseSessionAPI>().HeartbeatAsync());
    /// <summary>
    /// Refreshes the session cookie, extending the session lifetime by 5 minutes.
    /// Call this every 4–5 minutes while idle to prevent the OnBase session from expiring.
    /// </summary>
    public void Heatbeat() => HeartbeatAsync().Wait();

    /// <summary>
    /// Closes the OnBase session and releases the consumed license.
    /// Always disconnect when finished to avoid holding licenses unnecessarily.
    /// </summary>
    public Task DisconnectAsync() => Run(Api<IOnBaseSessionAPI>().DisconnectAsync());

    /// <summary>
    /// Closes the OnBase session and releases the consumed license.
    /// Always disconnect when finished to avoid holding licenses unnecessarily.
    /// </summary>
    public void Disconnect() => DisconnectAsync().Wait();

    /// <summary>
    /// Returns the Session Cookie containing the Session Id value and Expiration date.
    /// </summary>
    public ISessionCookie? Cookie => GetSessionCookie();
    /// <summary>
    /// Checks for a Session Cookie and if the Session is expired.
    /// </summary>
    public bool IsActive => Cookie != null ? !Cookie.Expired : false;

    internal static OnBaseSession Create(IOnBaseApp app)
        => new OnBaseSession(app);

    internal SessionCookie? GetSessionCookie()
    {
        if(App.ClientFactory.CookieContainer != null)
        {
            var cookie = App.ClientFactory.CookieContainer
            .GetCookies(new Uri(App.ClientOptions.ApiBaseUrl))
            .FirstOrDefault(c => c.Name == "Cookie.Session.OnBase.Hyland");
            if (cookie != null)
                return SessionCookie.Create(cookie);
        }        

        return null;
    }
}
