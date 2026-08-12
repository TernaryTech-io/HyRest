using Microsoft.Extensions.Logging;
using System.Reflection;


namespace HyRest.Session;

public sealed partial class OnBaseSession : OnBaseModule<IOnBaseSessionAPI>, IOnBaseSession
{
    private readonly HylandApiClient _apiClient;
    internal OnBaseSession(IOnBaseApp app) : base(app)
    {
        _apiClient = (HylandApiClient)app.ClientFactory.ApiClient;
    }
    /// <summary>
    /// Initiates the OnBase session and captures the session cookie by calling
    /// a lightweight endpoint. Call this right after creating the client to ensure
    /// the session is established before any document operations begin.
    /// </summary>
    public Task InitiateAsync(CancellationToken token = default) => Run(Api.InitiateSessionAsync(), token);

    /// <summary>
    /// Initiates the OnBase session and captures the session cookie by calling
    /// a lightweight endpoint. Call this right after creating the client to ensure
    /// the session is established before any document operations begin.
    /// </summary>
    public void Initiate() => InitiateAsync().Wait(App.ClientOptions.RequestTimeOut);

    /// <summary>
    /// Refreshes the session cookie, extending the session lifetime by 5 minutes.
    /// Call this every 4–5 minutes while idle to prevent the OnBase session from expiring.
    /// </summary>
    public Task HeartbeatAsync(CancellationToken token = default) => Run(Api.HeartbeatAsync(), token);
    /// <summary>
    /// Refreshes the session cookie, extending the session lifetime by 5 minutes.
    /// Call this every 4–5 minutes while idle to prevent the OnBase session from expiring.
    /// </summary>
    public void Heatbeat() => HeartbeatAsync().Wait(App.ClientOptions.RequestTimeOut);

    /// <summary>
    /// Closes the OnBase session and releases the consumed license.
    /// Always disconnect when finished to avoid holding licenses unnecessarily.
    /// </summary>
    public Task DisconnectAsync(CancellationToken token = default) => Run(Api.DisconnectAsync(), token);

    /// <summary>
    /// Closes the OnBase session and releases the consumed license.
    /// Always disconnect when finished to avoid holding licenses unnecessarily.
    /// </summary>
    public void Disconnect() => DisconnectAsync().Wait(App.ClientOptions.RequestTimeOut);

    /// <summary>
    /// Returns the Session Cookie containing the Session Id value and Expiration date.
    /// </summary>
    public ISessionCookie? Cookie => _apiClient.SessionCookie;
    /// <summary>
    /// Checks for a Session Cookie and if the Session is expired.
    /// </summary>
    public bool IsActive => _apiClient.IsActive;

    internal static OnBaseSession Create(IOnBaseApp app)
        => new OnBaseSession(app);    
}
