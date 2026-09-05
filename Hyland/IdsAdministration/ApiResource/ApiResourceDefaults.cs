using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HyRest.Hyland.IdentityAdministration;

internal static class ApiResourceDefault
{
    internal static ICollection<ApiResource> Default = JsonSerializer.Deserialize<ICollection<ApiResource>>(DefaultJson)
        ?? throw new Exception("Could not deserialize defaults");
    internal const string DefaultJson = $@"[
        {{
          ""Id"": ""1"",
          ""Name"": ""onbaseapi"",
          ""DisplayName"": ""OnBase APIs"",
          ""Enabled"": true,
          ""Scopes"": [
            ""onbaseapi""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""2"",
          ""Name"": ""hyland.fcs"",
          ""DisplayName"": ""File Conversion Service API"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hyland.fcs""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""3"",
          ""Name"": ""idpadmin"",
          ""DisplayName"": ""Identity Service Admin APIs"",
          ""Enabled"": true,
          ""Scopes"": [
            ""idpadmin""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant"",
            ""group""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""4"",
          ""Name"": ""group"",
          ""DisplayName"": ""Group"",
          ""Enabled"": true,
          ""Scopes"": [
            ""group""
          ],
          ""UserClaims"": [
            ""group""
          ],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""5"",
          ""Name"": ""evolution"",
          ""DisplayName"": ""Evolution APIs"",
          ""Enabled"": true,
          ""Scopes"": [
            ""evolution""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""6"",
          ""Name"": ""internal-system"",
          ""DisplayName"": ""Internal System Service Account"",
          ""Enabled"": true,
          ""Scopes"": [
            ""internal-system""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""7"",
          ""Name"": ""iam.user-catalog"",
          ""DisplayName"": ""User Catalog Interactions"",
          ""Enabled"": true,
          ""Scopes"": [
            ""iam.user-catalog"",
            ""iam.user-catalog.read"",
            ""iam.user-catalog.write""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""8"",
          ""Name"": ""hyland.keyvault"",
          ""DisplayName"": ""Hyland KeyVault"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hyland.keyvault""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""9"",
          ""Name"": ""cvat"",
          ""DisplayName"": ""Claim Validation and Transformation"",
          ""Enabled"": true,
          ""Scopes"": [
            ""cvat.bff.api"",
            ""cvat.client.bff"",
            ""cvat.demographics.api"",
            ""cvat.claims-hcfa.api"",
            ""cvat.claims-ub04.api""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""10"",
          ""Name"": ""fpa"",
          ""DisplayName"": ""Financial Process Automation"",
          ""Enabled"": true,
          ""Scopes"": [
            ""fpa""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""18"",
          ""Name"": ""hsp.account"",
          ""DisplayName"": ""HSP Account Related APIs"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hsp.account"",
            ""hsp.account.read"",
            ""hsp.account.write"",
            ""hsp.account.delete""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant"",
            ""group""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""Q/zs/KLxkh3/vdFv+bGM2EIv72I5ntsOQ8FmFZlNkk0="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""19"",
          ""Name"": ""hsp.index"",
          ""DisplayName"": ""HSP Index Related APIs"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hsp.index"",
            ""hsp.index.read"",
            ""hsp.index.write"",
            ""hsp.index.delete""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant"",
            ""group""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""Q/zs/KLxkh3/vdFv+bGM2EIv72I5ntsOQ8FmFZlNkk0="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""20"",
          ""Name"": ""hsp.document"",
          ""DisplayName"": ""HSP Document Related APIs"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hsp.document"",
            ""hsp.document.read"",
            ""hsp.document.write"",
            ""hsp.document.delete""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant"",
            ""group""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""Q/zs/KLxkh3/vdFv+bGM2EIv72I5ntsOQ8FmFZlNkk0="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""11"",
          ""Name"": ""efm"",
          ""DisplayName"": ""Employee File Management"",
          ""Enabled"": true,
          ""Scopes"": [
            ""efm""
          ],
          ""UserClaims"": [
            ""hsf_user""
          ],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""12"",
          ""Name"": ""healthcare.config"",
          ""DisplayName"": ""Healthcare Configuration Service"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hc.config.read"",
            ""hc.config.write""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""13"",
          ""Name"": ""mca"",
          ""DisplayName"": ""Metadata Content Abstraction"",
          ""Enabled"": true,
          ""Scopes"": [
            ""mca""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""14"",
          ""Name"": ""psw.content"",
          ""DisplayName"": ""Perceptive Content"",
          ""Enabled"": true,
          ""Scopes"": [
            ""psw.content""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""yAKOTk0nwbK7dcjFyI086uNrVs1R/wMgwB9m3Pj3x1U="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""15"",
          ""Name"": ""psw.preferences-service"",
          ""DisplayName"": ""Hyland Preferences Service"",
          ""Enabled"": true,
          ""Scopes"": [
            ""psw.preferences-service""
          ],
          ""UserClaims"": [
            ""sub"",
            ""tenant""
          ],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""UmLHk9XX0yU/gU8aARvojiI5UrhDYm5zerwClzFNh6c="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""16"",
          ""Name"": ""nilread"",
          ""DisplayName"": ""NilRead"",
          ""Enabled"": true,
          ""Scopes"": [
            ""nilread""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""17"",
          ""Name"": ""quantum.referencelogout"",
          ""DisplayName"": ""Reference Logout"",
          ""Enabled"": true,
          ""Scopes"": [
            ""quantum.referencelogout""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": [
            {{
              ""Description"": null,
              ""Value"": ""K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="",
              ""Expiration"": null,
              ""Type"": ""SharedSecret"",
              ""Hashed"": true
            }}
          ]
        }},
        {{
          ""Id"": ""21"",
          ""Name"": ""hcmisbe"",
          ""DisplayName"": ""Hyland Content Management Integration Services for Business Entities"",
          ""Enabled"": true,
          ""Scopes"": [
            ""hcmisbe""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }},
        {{
          ""Id"": ""22"",
          ""Name"": ""gis.config"",
          ""DisplayName"": ""GIS Configuration Service"",
          ""Enabled"": true,
          ""Scopes"": [
            ""gis.user"",
            ""gis.config""
          ],
          ""UserClaims"": [],
          ""ApiSecrets"": []
        }}
      ]";
}