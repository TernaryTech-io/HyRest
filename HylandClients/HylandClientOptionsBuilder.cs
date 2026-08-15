
namespace HyRest;

public class HylandOpenIdClientOptionsBuilder
{
    public required Action<IHylandClientOptions> OptionsAction { get; set; }
}
public class HylandClientOptionsBuilder
{
    public required IAuthenticationCredentials Credentials { get; set; }
    public required Action<IAuthenticationCredentials, IHylandClientOptions> OptionsAction { get; set; }
}