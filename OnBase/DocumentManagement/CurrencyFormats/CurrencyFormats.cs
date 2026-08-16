using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public sealed class CurrencyFormats : OnBaseItemTypeCollectionService<OnBaseCore, CurrencyFormat>
{

    internal CurrencyFormats(OnBaseCore core) : base(core) { }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetCurrencyFormats(token);
        col?.Items
                .Select(i => new CurrencyFormat(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<CurrencyFormat?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetCurrencyFormat(id, token);
        if (model != null)
            return new CurrencyFormat(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
