using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class CurrencyFormats : OnBaseItemTypeCollectionService<OnBaseCore, CurrencyFormat>
{

    internal CurrencyFormats(OnBaseCore core) : base(core) { }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.GetCurrencyFormatCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new CurrencyFormat(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
        base.GetCollection(token);
    }  
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
