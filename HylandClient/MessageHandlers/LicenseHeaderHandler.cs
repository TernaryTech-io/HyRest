using Microsoft.Extensions.Options;

namespace HyRest.Identity;

/// <summary>
/// Applies the License Header when Query API license is used.
/// </summary>
public sealed class LicenseHeaderHandler : DelegatingHandler
{
    private readonly IHylandClientOptions _options;
    public LicenseHeaderHandler(IOptions<HylandOpenIdOptionsBuilder> options)
        : base()
    {
        _options = new HylandClientOptions();
        options.Value.OptionsAction(_options);
    }
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        
        if (_options.UseQueryMetering)
            request.Headers.Add("Hyland-License-Type", "QueryMetering");
        
        return base.SendAsync(request, cancellationToken);
    }
}
