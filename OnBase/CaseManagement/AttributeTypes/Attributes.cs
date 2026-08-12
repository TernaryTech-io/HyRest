using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.CaseManagement;

public class Attributes : OnBaseItemTypeCollectionService<OnBaseWorkView, Attribute>
{    
    public Attributes(OnBaseWorkView module) : base(module)
    {
        
    }

    protected override async Task GetCollection(CancellationToken token = default)
    {
        Module.Classes.ToList()
            .ForEach(async c =>
            {
                var attributes = await Module.Run(Module.Api.Attributes(c.Id.ToString(), Module.App.ClientOptions.DefaultLanguage));
                _items.AddRange(attributes.Items.Select(a => new Attribute(Module, a)));
            });
        base.GetCollection(token);
    }    
}