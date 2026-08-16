
namespace HyRest;

public class HylandClientOptionsBuilder
{
    public required IAuthenticationCredentials Credentials { get; set; }
    public required Action<IAuthenticationCredentials, IHylandClientOptions> OptionsAction { get; set; }
}