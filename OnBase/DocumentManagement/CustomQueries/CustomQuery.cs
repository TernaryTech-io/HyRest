using HyRest.Utilities;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace HyRest.DocumentManagement;
public sealed class CustomQuery : OnBaseItemTypeService<IOnBaseDocumentAPI, OnBaseCore, CustomQueryModel>
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
                PopulateKeywordTypes().Wait();
            return _keywordTypes ?? [];
        }
    }
    private async Task PopulateKeywordTypes()
    {
        if (Item.Id != null)
        {
            var col = await Module.Run(Api.GetKeywordTypeCollectionForCustomQuery(Item.Id));
            if (col != null)
                col.Items
                .Select(i => Module.KeywordTypes.Find(i.Id))
                .ToList()
                .ForEach(i => _keywordTypes.Add(i));
        }
        else throw new Exception("The custom query id is missing.");
        
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
