using HyRest.Cache;
using HyRest.OnBase.ApiServices;
using Microsoft.Extensions.Logging;

namespace HyRest;

/// <summary>
/// Represents the connection to the OnBase related REST API.
/// </summary>
public partial class OnBaseApp : OnBaseAppBase
{
    #region private
    private bool _isInitated { get; set; }
    private User _currentUser { get; set; }
    #endregion
    /// <summary>
    /// Constructor for initiating a OnBase App through dependancy injection or the OnBaseAppBuilder
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="credentials"></param>
    /// <param name="options"></param>
    public OnBaseApp(ILogger<OnBaseApp> logger, HylandClientFactory clientFactory, OnBaseSessionService sessionService, 
        OnBaseAdministrationService administrationService, OnBaseCoreService coreService, OnBaseWorkViewService workViewService)
        : base(logger, clientFactory)
    {
        Session = new OnBaseSession(this, sessionService);        
        Core = new OnBaseCore(this, coreService);
        WorkView = new OnBaseWorkView(this, workViewService);
        try
        {
            Administration = new OnBaseAdministration(this, administrationService);
        }
        catch { }
        Init();
    }
    public override bool IsConnected => Session.IsActive;
    public override HylandClientFactory ClientFactory => (HylandClientFactory)base.ClientFactory;
    public override HylandClientOptions ClientOptions => (HylandClientOptions)base.ClientOptions;
    public User CurrentUser => _currentUser;
    public new OnBaseCore Core { get => (OnBaseCore)base.Core; set => base.Core = value;  }
    public new OnBaseSession Session { get => (OnBaseSession)base.Session; protected set => base.Session = value;  }
    public new OnBaseWorkView WorkView { get => (OnBaseWorkView)base.WorkView; protected set => base.WorkView = value; }
    public new OnBaseAdministration? Administration { get => (OnBaseAdministration?)base.Administration; protected set => base.Administration = value; }
    internal protected OnBaseApp Init()
    {                
        if (!IsConnected)
            Session.Initiate();
        _isInitated = true;
        if (ClientFactory.AuthClient.UserInfo != null)
        {
            try
            {
                if (ClientFactory.AuthClient.UserInfo.UserId != null)
                    _currentUser = Administration.Users[ClientFactory.AuthClient.UserInfo.UserId];
                else if (ClientFactory.AuthClient.UserInfo.UserName != null)
                    _currentUser = Administration.Users.FirstOrDefault(u => u.Name == ClientFactory.AuthClient.UserInfo.UserName.ToUpper());
                else if (ClientFactory.AuthClient.UserInfo.Email != null)
                    _currentUser = Administration.Users.FirstOrDefault(u => u.EmailAddress == ClientFactory.AuthClient.UserInfo.Email);
            }
            catch(Exception ex)
            {
                //user may not be authorized
            }

        }
        return this;
    }
}
