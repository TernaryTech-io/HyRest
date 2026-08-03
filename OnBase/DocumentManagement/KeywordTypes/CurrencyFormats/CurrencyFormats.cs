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
    protected override async Task<IOnBaseItemTypeService?> GetOne(string identifier)
    {
        if (long.TryParse(identifier, out long id))
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
                return item;
            else
            {
                var model = await Module.Run(Api.GetCurrencyFormatById(id.ToString(), Options.DefaultLanguage));
                if (model != null)
                {
                    var newItem = new CurrencyFormat(Module, model);
                    Add(newItem);
                    return newItem;
                }
            }
        }
        else
        {
            var item = _items.FirstOrDefault(i => i.SystemName == identifier);
            if (item != null)
                return item;
            else
            {
                var col = await Module.Run(Api.GetCurrencyFormatCollection(null, [identifier], Options.DefaultLanguage));
                if (col != null && col.Items.FirstOrDefault() != null)
                {
                    var model = col.Items.FirstOrDefault();
                    var newItem = new CurrencyFormat(Module, model);
                    Add(newItem);
                    return newItem;
                }
            }
        }
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
