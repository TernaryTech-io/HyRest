using HyRest.Hyland.IdentityAdministration;
using System.ComponentModel;

namespace HyRest;
public class SCIMCredentials : AuthenticationCredentials
{
    public new GrantType GrantType => GrantType.ClientCredentials;
    public override List<Scope> Scopes => [ Scope.IamUserCatalog, Scope.IamUserCatalogRead, Scope.IamUserCatalogWrite ];

}
public class ReadOnlySCIMCredentials : AuthenticationCredentials
{
    public new GrantType GrantType => GrantType.ClientCredentials;
    public override List<Scope> Scopes => [ Scope.IamUserCatalog, Scope.IamUserCatalogRead ];
}