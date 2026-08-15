using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.OnBase.WorkView;

public class Attributes : OnBaseItemTypeCollectionService<OnBaseWorkView, Attribute>
{    
    public Attributes(OnBaseWorkView module) : base(module)
    {
        
    }

    protected override async Task GetCollection(CancellationToken token = default)
    {
        foreach (var app in Module.Classes.ToList())
        {
            var col = await Module.Service.GetAttributes(app.Id.ToString(), token);
            col?.Items
                    .Select(i => new Attribute(Module, i))
                    .ToList()
                    .ForEach(i => Add(i));
        }
    }
    protected override async Task<Attribute?> GetOne(string id, CancellationToken token = default)
    {
        foreach (var cls in Module.Classes.ToList())
        {
            if (cls.Attributes.Any(a => a.Id.ToString() == id))
            {
                var model = await Module.Service.GetAttribute(cls.Id.ToString(), id, token);
                if (model != null)
                    return new Attribute(Module, model);
            }
        }

        return null;
    }
}