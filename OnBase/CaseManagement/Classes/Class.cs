using System.Text.Json.Serialization;
using Ternary.DataConversions.Extensions;

namespace HyRest.CaseManagement;

public class Class : OnBaseItemTypeService<IOnBaseWorkViewAPI, OnBaseWorkView, ClassModel>
{
    private ClassAccessRights? _accessRights { get; set; }
    private List<Attribute> _attributes { get; set; } = [];
    //private List<Filter>
    public Class(OnBaseWorkView module, ClassModel item) : base(module, item)
    {

    }
    public long Id => Item.Id.ConvertTo<long>();
    public string Name => Item.Name ?? string.Empty;
    public string SystemName => Item.SystemName ?? string.Empty;
    /// <summary>
    /// The base most Class Id of the current class.
    /// </summary>
    public long RootClassId => Item.RootClassId.ConvertTo<long>();
    public IReadOnlyCollection<Attribute> Attributes
    {
        get
        {
            if(_attributes.Count == 0)
                GetClassAttributes().Wait();
            return _attributes.AsReadOnly();
        }
    }
    public ClassAccessRights AccessRights
    {
        get
        {
            if (_accessRights == null)
                GetAccessRights().Wait();
            return _accessRights;
        }
    }
    private async Task GetAccessRights()
    {
        if (_accessRights == null)
        {
            _accessRights = await Module.Run(Api.AccessRights(Item.Id));
        }
    }
    private async Task GetClassAttributes()
    {
        var attributes = await Module.Run(Api.Attributes(Item.Id, Module.App.ClientOptions.DefaultLanguage));
        if(attributes != null)
            _attributes = attributes.Items.Select(a => new Attribute(Module, a)).ToList();
    }
}
