using Ternary.DataConversions.Extensions;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class DocumentResult
{
    private readonly OnBaseCore _core;
    private readonly long _id;
    private List<QueryDisplayColumn> _displayColumns { get; set; } = [];
    private Document? _document { get; set; }
    public DocumentResult(OnBaseCore core, DocumentResultModel docresultModel)
    {
        _core = core;
        _id = docresultModel.Id.ConvertTo<long>();
        _displayColumns = docresultModel.DisplayColumns
            .Select(c => new QueryDisplayColumn(c))
            .ToList();
    }    
    public long DocumentId { get => _id; } 
    public Document? Document
    {
        get
        {
            if (_document == null)
                GetDocument().Wait();
            return _document;
        }
    }
    private async Task GetDocument()
    {
        _document = await _core.GetDocumentByIdAsync(_id);
    }
    public IReadOnlyCollection<QueryDisplayColumn> DisplayColumns { get => _displayColumns.AsReadOnly(); }
    public string? ToJson()
        => JsonUtility.Serialize(this);
}

public class QueryDisplayColumn
{
    private readonly int _index;
    private List<string> _values { get; set; } = [];
    public QueryDisplayColumn(DisplayColumnModel columnModel)
    {
        _index = columnModel.Index.ConvertTo<int>();
        _values = columnModel.Values.ToList();
    }
    public int Index { get => _index; }
    public IReadOnlyCollection<string> Values { get => _values.AsReadOnly(); }
    public string? ToJson()
        => JsonUtility.Serialize(this);
}