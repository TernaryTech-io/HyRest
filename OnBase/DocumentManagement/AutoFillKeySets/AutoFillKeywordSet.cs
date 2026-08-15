using HyRest.Utilities;
using System.Text.Json.Serialization;

namespace HyRest.OnBase.Core;

public sealed class AutoFillKeywordSet : OnBaseItemTypeService<OnBaseCore,AutoFillKeywordSetModel>
{

    private KeywordType? _primaryKeywordType { get; set; }   
    private List<KeywordType> _keywordTypes { get; set; }
    internal AutoFillKeywordSet(OnBaseCore core, AutoFillKeywordSetModel item) : base(core, item) { }
    public KeywordType? PrimaryKeywordType
    {
        get
        {
            if (_primaryKeywordType == null)
                PopulatePrimaryKeywordType().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _primaryKeywordType;
        }
    }
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
    public bool External => Item.External;
    private async Task PopulatePrimaryKeywordType()
    {
        if(Item.PrimaryKeywordTypeId != null)
        {
            var item = await Module.Service.GetKeywordType(Item.PrimaryKeywordTypeId);
            if (item != null)
                _primaryKeywordType = new KeywordType(Module, item);
        }        
    }
    private async Task PopulateKeywordTypes()
    {
        var col = await Module.Service.GetAutoFillKeywordSetKeywordTypes(Item.Id);
        col?.Items
            .Select(i => new KeywordType(Module, i))
            .ToList()
            .ForEach(i => _keywordTypes.Add(i));
    }
    //public async Task<IReadOnlyCollection<Keyword>> GetAutoFillData(string primaryValue, CancellationToken token = default)
    //{
    //    var data = await Module.Run(Module.Api.GetKeywordDataCollectionForAutofillKeywordSet(Item.Id, primaryValue, Module.App.ClientOptions.DefaultLanguage), token);
    //    List<Keyword> results = [];
    //    foreach(var item in data.Items)
    //    {
    //        item.
    //    }

    //}
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
