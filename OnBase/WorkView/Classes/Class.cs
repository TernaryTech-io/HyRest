using System.Text.Json.Serialization;
using Ternary.DataConversions.Extensions;

namespace HyRest.OnBase.WorkView;

public class Class : OnBaseItemTypeService<OnBaseWorkView, ClassModel>
{
    private ClassAccessRights? _accessRights { get; set; }
    private List<Attribute> _attributes { get; set; } = [];
    //private List<Filter>
    public Class(OnBaseWorkView module, ClassModel item) : base(module, item)
    {

    }
    /// <summary>
    /// The base most Class Id of the current class.
    /// </summary>
    public long RootClassId => Item.RootClassId.ConvertTo<long>();
    [JsonIgnore]
    public IReadOnlyCollection<Attribute> Attributes
    {
        get
        {
            if(_attributes.Count == 0)
                GetClassAttributes().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _attributes.AsReadOnly();
        }
    }
    public ClassAccessRights AccessRights
    {
        get
        {
            if (_accessRights == null)
                GetAccessRights().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _accessRights;
        }
    }
    private async Task GetAccessRights()
    {
        if (_accessRights == null)
        {
            _accessRights = await Module.Service.GetAccessRights(Item.Id);
        }
    }
    private async Task GetClassAttributes(CancellationToken token = default)
    {
        var attributes = await Module.Service.GetAttributes(Item.Id, token);
        if(attributes != null)
            _attributes = attributes.Items.Select(a => new Attribute(Module, a)).ToList();
    }
}
