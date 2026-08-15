using System.Text.Json.Serialization;
using Ternary.DataConversions.Extensions;

namespace HyRest.OnBase.WorkView;

public class Application : OnBaseItemTypeService<OnBaseWorkView, ApplicationModel>
{
    private List<Class> _classes { get; set; } = [];
    internal Application(OnBaseWorkView module, ApplicationModel item) : base(module, item)
    {

    }
    /// <summary>
    /// Description of this Application.
    /// </summary>
    public string? Description => Item.Description;

    /// <summary>
    /// Identifier of the catalog to use for Full Text searching for this Application.
    /// </summary>
    public long FullTextCatalogId => Item.FullTextCatalogId.ConvertTo<long>();

    /// <summary>
    /// Identifier of the default Filter for this Application.
    /// </summary>
    public long DefaultFilterId => Item.DefaultFilterId.ConvertTo<long>();
    [JsonIgnore]
    public IReadOnlyCollection<Class> Classes
    {
        get
        {
            if (_classes.Count == 0)
                PopulateClasses().Wait(Module.App.RequestTimeOut);
            return _classes;
        }
    }
    private async Task PopulateClasses()
    {
        var classes = await Module.Service.GetClasses(Item.Id);
        _classes.AddRange(classes.Items.Select(i =>  new Class(Module, i)));
    }
}
