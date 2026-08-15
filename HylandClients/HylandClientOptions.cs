using Microsoft.Extensions.Logging;

namespace HyRest;

public class HylandClientOptions : IHylandClientOptions
{
    /// <summary>
    /// The base url of your Hyland Identity server, ex: https://onbase.server.com/IdentityServer
    /// </summary>
    public string IdsBaseUrl { get; set; }
    /// <summary>
    /// The base url of your Hyland RestApi server, ex: https://onbase.server.com/ApiServer
    /// </summary>
    public string ApiBaseUrl { get; set; }
    /// <summary>
    /// Set to True if using QueryMetering API license, false to use standard concurrent / named licenses
    /// </summary>
    public bool UseQueryMetering { get; set; }
    /// <summary>
    /// Set the deafualt language to be used, en-US is default
    /// </summary>
    public string DefaultLanguage { get; set; } = "en-US";
    /// <summary>
    /// Sets the API server timeout in seconds.
    /// </summary>
    public int RequestTimeOut { get; set; } = 120;
    /// <summary>
    /// Set the default logging level.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
    public static HylandClientOptions Create(string idsBaseUrl, string apiBaseUrl,
        bool useQueryMetering = false, string defaultLanguage = "en-US", int requestTimeOut = 120, LogLevel loglevel = LogLevel.Information) => new HylandClientOptions
        {
            IdsBaseUrl = idsBaseUrl,
            ApiBaseUrl = apiBaseUrl,
            UseQueryMetering = useQueryMetering,
            DefaultLanguage = defaultLanguage,    
            RequestTimeOut = requestTimeOut,
            LogLevel = loglevel
        };
}

