using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class KeywordTypeCollection : OnBaseRestService
{
    private readonly KeywordTypeGroupCollectionModel _model;
    private KeywordOptions _keywordOptions { get; set; }
    private OnBaseCore _core => (OnBaseCore)base.Module;
    private StandAloneKeywordTypes? _standAloneKeywordTypes { get; set; }
    private List<SingleInstanceKeywordTypeGroup> _singleInstanceKeywordTypeGroups { get; set; } = [];
    private List<MultiInstanceKeywordTypeGroup> _multiInstanceKeywordTypeGroups { get; set; } = [];
    internal KeywordTypeCollection(OnBaseCore core, KeywordTypeGroupCollectionModel item) : base(core)
    {
        _model = item;
        foreach(var group in _model.Items)
        {
            if(string.IsNullOrEmpty(group.Id))
            {
                List<KeywordType> keywordTypes = [];
                foreach(var type in group.KeywordTypes)
                {
                    var kt = _core.KeywordTypes.Find(type.Id);
                    if (kt != null)
                        keywordTypes.Add(kt);
                }
                _standAloneKeywordTypes = new StandAloneKeywordTypes(_core, keywordTypes);
            }
            else
            {
                var ktg = _core.KeywordTypeGroups.Find(group.Id);

                if (ktg != null && ktg.StorageType == KeywordTypeGroupType.SingleInstance)
                    _singleInstanceKeywordTypeGroups.Add((SingleInstanceKeywordTypeGroup)ktg);
                else if(ktg != null && ktg.StorageType == KeywordTypeGroupType.MultiInstance)
                    _multiInstanceKeywordTypeGroups.Add((MultiInstanceKeywordTypeGroup)ktg);
            }
        }
    }
    public KeywordOptions KeywordOptions
    {
        get
        {
            if (_keywordOptions == null)
                _keywordOptions = new KeywordOptions(_core, _model.KeywordOptions);
            return _keywordOptions;
        }
    }
    public StandAloneKeywordTypes StandAloneKeywordTypes => _standAloneKeywordTypes ?? new(_core, []);
    public IReadOnlyCollection<SingleInstanceKeywordTypeGroup> SingleInstanceKeywordTypeGroups => _singleInstanceKeywordTypeGroups;
    public IReadOnlyCollection<MultiInstanceKeywordTypeGroup> MultiInstanceKeywordTypeGroups => _multiInstanceKeywordTypeGroups;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}


