using HyRest.Utilities;

namespace HyRest.OnBase.Core;
public sealed class CustomQueries : OnBaseItemTypeCollectionService<OnBaseCore, CustomQuery>
{
    internal CustomQueries(OnBaseCore core) : base(core) { }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetCustomQueries(token);
        col?.Items
                .Select(i => new CustomQuery(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<CustomQuery?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetCustomQuery(id, token);
        if (model != null)
            return new CustomQuery(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
