using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.CaseManagement;

public class Attributes : OnBaseItemTypeCollectionService<IOnBaseWorkViewAPI, OnBaseWorkView, Attribute>
{    
    public Attributes(OnBaseWorkView module) : base(module)
    {
        
    }

    protected override async Task GetCollection()
    {
        foreach(var cls in Module.Classes.ToList())
        {
            var attributes = await Module.Run(Api.Attributes(cls.Id.ToString(), Module.App.ClientOptions.DefaultLanguage));
            _items.AddRange(attributes.Items.Select(a => new Attribute(Module, a)));
        }
    }
}