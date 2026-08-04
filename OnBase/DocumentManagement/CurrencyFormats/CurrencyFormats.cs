using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class CurrencyFormats : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, CurrencyFormat>
{

    internal CurrencyFormats(OnBaseCore core) : base(core) { }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetCurrencyFormatCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new CurrencyFormat(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }  
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
