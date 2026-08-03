using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class AutoFillKeywordSet : OnBaseItemTypeService<IOnBaseDocumentAPI,OnBaseCore,AutoFillKeywordSetModel>
{

    private KeywordType? _primaryKeywordType { get; set; }   
    private List<KeywordType> _keywordTypes { get; set; }
    internal AutoFillKeywordSet(OnBaseCore core, AutoFillKeywordSetModel item) : base(core, item) { }
    public KeywordType? PrimaryKeywordType
    {
        get
        {
            if (_primaryKeywordType == null)
                PopulatePrimaryKeywordType().Wait();
            return _primaryKeywordType;
        }
    }
    public List<KeywordType> KeywordTypes
    {
        get
        {
            if (_keywordTypes == null || _keywordTypes.Count == 0)
                PopulateKeywordTypes().Wait();
            return _keywordTypes;
        }
    }
    public bool External => Item.External;
    private async Task PopulatePrimaryKeywordType()
    {
        if(Item.PrimaryKeywordTypeId != null)
        {
            var item = await Module.Run(Api.GetKeywordTypeById(Item.PrimaryKeywordTypeId, Options.DefaultLanguage));
            if (item != null)
                _primaryKeywordType = new KeywordType(Module, item);
        }        
    }
    private async Task PopulateKeywordTypes()
    {
        var col = await Module.Run(Api.GetKeywordTypeCollectionForAutofillKeywordSet(Item.Id));
        if(col != null)
            col.Items
            .Select(i => new KeywordType(Module, i))
            .ToList()
            .ForEach(i => _keywordTypes.Add(i));
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
