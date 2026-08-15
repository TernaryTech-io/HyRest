using HyRest.Utilities;
using System.Text.Json.Serialization;

namespace HyRest.OnBase.Core;
public sealed class CustomQuery : OnBaseItemTypeService<OnBaseCore, CustomQueryModel>
{
    private List<KeywordType> _keywordTypes { get; set; } = [];
    internal CustomQuery(OnBaseCore core, CustomQueryModel item) : base(core, item){}
    public string Instructions => Item.Instructions ?? string.Empty;
    public CustomQueryDateOptions? DateOptions => Item.DateOptions;
    public CustomQueryQueryType QueryType => Item.QueryType;
    [JsonIgnore]
    public IReadOnlyCollection<KeywordType> KeywordTypes 
    {
        get
        {
            if (_keywordTypes == null || _keywordTypes.Count == 0)
                PopulateKeywordTypes().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _keywordTypes ?? [];
        }
    }
    private async Task PopulateKeywordTypes()
    {
        if (Item.Id != null)
        {
            var col = await Module.Service.GetKeywordsForCustomQuery(Item.Id);
            col?.Items
                .Select(i => Module.KeywordTypes[i.Id])
                .ToList()
                .ForEach(i => _keywordTypes.Add(i));
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
