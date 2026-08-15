using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class DocumentQuery : OnBaseRestService
{
    private OnBaseCore _core => (OnBaseCore)Module;
    private readonly string _queryId;
    private int _resultsCount;
    private List<DocumentResult> _results { get; set; }
    public DocumentQuery(OnBaseCore core, string guid, int resultCount = 0) : base(core)
    {
        _queryId = guid;
        _resultsCount = resultCount;
    }
    /// <summary>
    /// Query Id returned when the query is created.
    /// </summary>
    public Guid QueryId { get => Guid.Parse(_queryId); }
    /// <summary>
    /// Returns when the query is created if that option was selected. 
    /// </summary>
    public int ResultsCount { get => _resultsCount; }

    public IReadOnlyCollection<DocumentResult> Results { get => _results.AsReadOnly(); }
    /// <summary>
    /// Execute the query asynchronously and return a collection of results. 
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<DocumentResult>> GetResultsAsync(CancellationToken token = default)
    {
        var resp = await _core.Service.GetQueryResults(_queryId, token);
        if(resp != null)
        {
            _results = resp.Items.Select(r => new DocumentResult(_core, r)).ToList();
        }
        return Results;
    }
    public IReadOnlyCollection<DocumentResult> GetResults()
    {
        GetResultsAsync().Wait(Module.App.ClientOptions.RequestTimeOut);
        return Results;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}

