using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class KeywordOptions : OnBaseRestService
{
    private readonly KeywordOptionsModel _model;
    private OnBaseCore _core => (OnBaseCore)base.Module;
    private List<KeywordType> _requiredArchival { get; set; } = [];
    private List<KeywordType> _requiredRetreival { get; set; } = [];
    private List<KeywordType> _readonly { get; set; } = [];
    public KeywordOptions(OnBaseCore core, KeywordOptionsModel item) : base(core)
    {
        _model = item;
    }
    /// <summary>
    /// An array of required keyword types for a document to be stored.
    /// </summary>
    public IReadOnlyList<KeywordType> RequiredForArchivalKeywordTypeIds 
    { 
        get
        {
            if (_requiredArchival.Count == 0)
                PopulateRequiredForArchival();
            return _requiredArchival;
        }
    }

    /// <summary>
    /// An array of required keyword types for a document to be retrieved.
    /// </summary>
    public IReadOnlyList<KeywordType> RequiredForRetrievalKeywordTypeIds
    {
        get
        {
            if (_requiredRetreival.Count == 0)
                PopulateRequiredForRetrieval();
            return _requiredRetreival;
        }
    }

    /// <summary>
    /// An array of read only keyword types for a document type.
    /// </summary>
    public IReadOnlyList<KeywordType> ReadOnlyKeywordTypeIds
    {
        get
        {
            if (_readonly.Count == 0)
                PopulateReadOnly();
            return _readonly;
        }
    }

    public void PopulateRequiredForArchival()
    {
        foreach(var item in _model.RequiredForArchivalKeywordTypeIds)
        {
            var kt = _core.KeywordTypes.Find(item);
            if (kt != null)
                _requiredArchival.Add(kt);
        }
    }
    public void PopulateRequiredForRetrieval()
    {
        foreach (var item in _model.RequiredForRetrievalKeywordTypeIds)
        {
            var kt = _core.KeywordTypes.Find(item);
            if (kt != null)
                _requiredRetreival.Add(kt);
        }
    }
    public void PopulateReadOnly()
    {
        foreach (var item in _model.ReadOnlyKeywordTypeIds)
        {
            var kt = _core.KeywordTypes.Find(item);
            if (kt != null)
                _readonly.Add(kt);
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
