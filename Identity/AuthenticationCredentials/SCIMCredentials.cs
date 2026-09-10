using HyRest.Hyland.IdentityAdministration;
using System.ComponentModel;

namespace HyRest;
public class SCIMCredentials : AuthenticationCredentials
{
    public override GrantType GrantType { get; set; } =  GrantType.ClientCredentials;
    public override List<Scope> Scopes { get; set; } = [ Scope.IamUserCatalog, Scope.IamUserCatalogRead, Scope.IamUserCatalogWrite ];

}
public class ReadOnlySCIMCredentials : AuthenticationCredentials
{
    public override GrantType GrantType { get; set; } = GrantType.ClientCredentials;
    public override List<Scope> Scopes { get; set; } = [ Scope.IamUserCatalog, Scope.IamUserCatalogRead ];
}