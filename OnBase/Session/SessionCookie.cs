using System.Net;

namespace HyRest.Session;

public class SessionCookie : ISessionCookie
{
    private Cookie _cookie { get; set; }
    internal SessionCookie(Cookie cookie)
    {
        _cookie = cookie;
    }
    public string SessionId => _cookie.Value;
    public DateTime Expiration => _cookie.Expires;
    public bool Expired => _cookie.Expired;
    public static SessionCookie Create(Cookie cookie)
        => new SessionCookie(cookie);    
}
