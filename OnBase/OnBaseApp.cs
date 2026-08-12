using HyRest.Administration;
using HyRest.Cache;
using HyRest.CaseManagement;
using HyRest.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HyRest;

/// <summary>
/// Represents the connection to the OnBase related REST API.
/// </summary>
public partial class OnBaseApp : OnBaseAppBase
{
    #region private
    private bool _isInitated { get; set; }
    private IHylandClientOptions _options;
    private ILogger<OnBaseApp> _logger;
    private IHylandClientFactory _clientFactory;
    private IOnBaseAppCache _cache;
    private User _currentUser { get; set; }
    private OnBaseCore _core { get => (OnBaseCore)base.Core; set => base.Core = value; }
    private OnBaseSession _session { get => (OnBaseSession)base.Session; set => base.Session = value; }
    private OnBaseWorkView _workView { get => (OnBaseWorkView)base.WorkView; set => base.WorkView = value; }
    private OnBaseAdministration _administration { get => (OnBaseAdministration)base.Administration; set => base.Administration = value; }
    #endregion
    /// <summary>
    /// Constructor for initiating a OnBase App with specified credentials.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="credentials"></param>
    /// <param name="options"></param>
    public OnBaseApp(ILogger<OnBaseApp> logger, IHylandClientFactory clientFactory, HylandClientOptions options, OnBaseAppCache cache)
    {
        _clientFactory = clientFactory;
        _options = options;
        _logger = logger;
        _cache = cache;
        Init();
    }
    /// <summary>
    /// Constructor for initiating an OnBase App through dependency injection.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="clientFactory"></param>
    /// <param name="options"></param>
    public OnBaseApp(ILogger<OnBaseApp> logger, IHylandClientFactory clientFactory, IOptions<HylandOpenIdClientOptionsBuilder> options, OnBaseAppCache cache)
    {
        _logger = logger;
        var clientOptions = new HylandClientOptions();
        options.Value.OptionsAction(clientOptions);
        _options = clientOptions;
        _clientFactory = clientFactory;
        _cache = cache;
        Init();
    }
    public override OnBaseAppCache Cache => (OnBaseAppCache)_cache;
    public override bool IsConnected => _session != null ? _session.IsActive : false;
    public override HylandClientFactory ClientFactory => (HylandClientFactory)_clientFactory;
    public override HylandClientOptions ClientOptions => (HylandClientOptions)_options;
    public User CurrentUser => _currentUser;
    public override OnBaseCore Core => _core;
    public override OnBaseSession Session => _session;
    public override OnBaseWorkView WorkView => _workView;
    public override OnBaseAdministration Administration => _administration;
    public override ILogger<IOnBaseApp> Logger => _logger;
    internal protected OnBaseApp Init()
    {                
        _session = OnBaseSession.Create(this);
        _core = OnBaseCore.Create(this);
        _workView = OnBaseWorkView.Create(this);
        _administration = OnBaseAdministration.Create(this);
        if (!IsConnected)
            Session.Initiate();
        _isInitated = true;
        if (_clientFactory.AuthClient.UserInfo != null)
        {
            if (_clientFactory.AuthClient.UserInfo.UserId != null)
                _currentUser = Administration.Users[_clientFactory.AuthClient.UserInfo.UserId];
            else if (_clientFactory.AuthClient.UserInfo.UserName != null)
                _currentUser = Administration.Users.FirstOrDefault(u => u.Name == _clientFactory.AuthClient.UserInfo.UserName.ToUpper());
            else if(_clientFactory.AuthClient.UserInfo.Email != null)
                _currentUser = Administration.Users.FirstOrDefault(u => u.EmailAddress == _clientFactory.AuthClient.UserInfo.Email);
        }
        return this;
    }
}
