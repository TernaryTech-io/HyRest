using HyRest.Utilities;

namespace HyRest.DocumentManagement;

/// <summary>
/// Represents the information required to execute a custom query.
/// </summary>
public class CustomQueryBuilder : DocumentQueryBuilder<CustomQuery, CustomQueryBuilder>
{
    public CustomQueryBuilder(OnBaseCore core) : base(core) { }
    public override QueryType Type => QueryType.CustomQuery;
    public override CustomQueryBuilder AddItem(string name)
    {
        var dt = _core.CustomQueries.Find(name);
        if (dt == null)
            throw new Exception($"Could not locate a custom query with name {name}");
        return AddItem(dt);
    }
    public override CustomQueryBuilder AddItem(long id)
    {
        var dt = _core.CustomQueries.Find(id);
        if (dt == null)
            throw new Exception($"Could not locate a custom query with id {id}");
        return AddItem(dt);
    }
    public string? ToJson()
        => JsonUtility.Serialize(this);
}
/// <summary>
/// Represents the information required to execute a document type group query.
/// </summary>
public class DocumentTypeGroupQueryBuilder : DocumentQueryBuilder<DocumentTypeGroup, DocumentTypeGroupQueryBuilder>
{
    public DocumentTypeGroupQueryBuilder(OnBaseCore core) : base(core) { }
    public override QueryType Type => QueryType.DocumentTypeGroup;
    public override DocumentTypeGroupQueryBuilder AddItem(string name)
    {
        var dt = _core.DocumentTypeGroups.Find(name);
        if (dt == null)
            throw new Exception($"Could not locate a document type group with name {name}");
        return AddItem(dt);
    }
    public override DocumentTypeGroupQueryBuilder AddItem(long id)
    {
        var dt = _core.DocumentTypeGroups.Find(id);
        if (dt == null)
            throw new Exception($"Could not locate a document type group with id {id}");
        return AddItem(dt);
    }
    public string? ToJson()
        => JsonUtility.Serialize(this);
}
/// <summary>
/// Represents the information required to execute a document type query.
/// </summary>
public class DocumentTypeQueryBuilder : DocumentQueryBuilder<DocumentType, DocumentTypeQueryBuilder>
{
    public DocumentTypeQueryBuilder(OnBaseCore core) : base(core) { }
    public override QueryType Type => QueryType.DocumentType;
    public override DocumentTypeQueryBuilder AddItem(string name)
    {
        var dt = _core.DocumentTypes.Find(name);
        if (dt == null)
            throw new Exception($"Could not locate a document type with name {name}");
        return AddItem(dt);
    }
    public override DocumentTypeQueryBuilder AddItem(long id)
    {
        var dt = _core.DocumentTypes.Find(id);
        if (dt == null)
            throw new Exception($"Could not locate a document type with id {id}");
        return AddItem(dt);
    }
    public string? ToJson()
        => JsonUtility.Serialize(this);
}

public abstract class DocumentQueryBuilder<TItem, TQuery> : DocumentQueryBuilder, IDocumentQueryBuilder
    where TItem : class, IOnBaseItemTypeService
    where TQuery : class, IDocumentQueryBuilder
{
    protected readonly OnBaseCore _core;
    private List<QueryKeyword> _queryKeywords { get; set; } = [];
    private List<DateRange> _documentDateRanges { get; set; } = [];
    private List<UserDefinedDisplayColumn> _userDisplayColumns { get; set; } = [];
    internal DocumentQueryBuilder(OnBaseCore core)
    {
        _core = core;
    }
    public override IReadOnlyCollection<TItem> Items => _items.Select(i => (TItem)i).ToList().AsReadOnly();
    IReadOnlyCollection<IOnBaseItemTypeService> IDocumentQueryBuilder.Items => Items;
    public override IReadOnlyCollection<QueryKeyword> QueryKeywords { get => _queryKeywords; }
    public override IReadOnlyCollection<DateRange> DocumentDateRanges { get => _documentDateRanges; }
    public override IReadOnlyCollection<UserDefinedDisplayColumn> UserDisplayColumns { get => _userDisplayColumns; }
    IDocumentQueryBuilder IDocumentQueryBuilder.AddItem(IOnBaseItemTypeService item)
        => AddItem(item);
    public TQuery AddItem(TItem item)
    {
        base.AddItem(item);
        return this as TQuery;
    }
    public abstract TQuery AddItem(long id);
    public abstract TQuery AddItem(string name);
    public TQuery AddQueryKeyword(Action<QueryKeyword> keywordBuilder)
    {
        var queryKeyword = new QueryKeyword(_core);
        keywordBuilder.Invoke(queryKeyword);
        _queryKeywords.Add(queryKeyword);
        return this as TQuery;
    }
    public override TQuery AddQueryKeyword(string keywordTypeName, object value, QueryKeywordOperator queryOperator, QueryKeywordRelation queryRelation)
    {
        var queryKeyword = new QueryKeyword(_core)
        {
            Id = keywordTypeName,
            Value = value,
            Operator = queryOperator,
            Relation = queryRelation,
        };
        _queryKeywords.Add(queryKeyword);
        return this as TQuery;
    }
    public override TQuery AddQueryKeyword(long keywordTypeId, object value, QueryKeywordOperator queryOperator, QueryKeywordRelation queryRelation)
    {
        var queryKeyword = new QueryKeyword(_core)
        {
            Id = keywordTypeId.ToString(),
            Value = value,
            Operator = queryOperator,
            Relation = queryRelation,
        };
        _queryKeywords.Add(queryKeyword);
        return this as TQuery;
    }
    
    internal QueryInformationModel GetModel()
    {
        return new QueryInformationModel()
        {
            QueryType = [new QueryTypeModel
                {
                    Type = this.Type,
                    Ids = _items.Select(i => i.Id.ToString()).ToList()
                }],
            MaxResults = MaxResults,
            DocumentDateRangeCollection = _documentDateRanges,
            QueryKeywordCollection = _queryKeywords.Select(k => k.GetModel()).ToList(),
            UserDisplayColumns = _userDisplayColumns
        };
    }
    public TQuery WithMaxResults(int maxResults)
    {
        MaxResults = maxResults;
        return this as TQuery;
    }
    public string? ToJson()
        => JsonUtility.Serialize(this);
    public override async Task<DocumentQuery> CreateQueryAsync(bool includeItemCount = false)
    {
        var resp = await _core.Run(_core.Api<IOnBaseDocumentAPI>().PostDocumentQuery(GetModel(), includeItemCount));
        if (resp != null)
        {
            int count = 0;
            if (resp.AdditionalProperties.ContainsKey("hyland-item-count"))
            {
                var countHeader = resp.AdditionalProperties["hyland-item-count"];
                if (countHeader is IEnumerable<string> strings && strings.Count() > 0
                    && int.TryParse(strings.First(), out count))
                {

                }
            }
            return new DocumentQuery(_core, resp.Id, count);
        }
        else throw new Exception("Failied to create the document query.");
    }
    public override DocumentQuery CreateQuery(bool includeItemCount = false)
    {
        var dqTask = CreateQueryAsync(includeItemCount);
        dqTask.Wait();
        if (dqTask.IsCompletedSuccessfully)
            return dqTask.Result;
        else throw dqTask.Exception ?? new Exception("Failed to intiate document query.");
    }
    public override TQuery AddDateRange(DateRange dateRange)
    {
        _documentDateRanges.Add(dateRange);
        return this as TQuery;
    }
    public override TQuery AddDateRange(DateTime start, DateTime end)
    {
        _documentDateRanges.Add(new DateRange { Start = start, End = end });
        return this as TQuery;
    }
}
public abstract class DocumentQueryBuilder : IDocumentQueryBuilder
{
    protected List<IOnBaseItemTypeService> _items { get; set; } = [];
    public abstract QueryType Type { get; }
    public int MaxResults { get; set; } = 1000;
    public abstract IReadOnlyCollection<IOnBaseItemTypeService> Items { get; }
    public abstract IReadOnlyCollection<QueryKeyword> QueryKeywords { get; }
    public abstract IReadOnlyCollection<DateRange> DocumentDateRanges { get; }
    public abstract IReadOnlyCollection<UserDefinedDisplayColumn> UserDisplayColumns { get; }
    public virtual IDocumentQueryBuilder AddItem(IOnBaseItemTypeService item)
    {
        _items.Add(item);
        return this;
    }
    public abstract IDocumentQueryBuilder AddQueryKeyword(string keywordTypeName, object value, QueryKeywordOperator queryOperator, QueryKeywordRelation queryRelation);
    public abstract IDocumentQueryBuilder AddQueryKeyword(long keywordTypeId, object value, QueryKeywordOperator queryOperator, QueryKeywordRelation queryRelation);
    public abstract IDocumentQueryBuilder AddDateRange(DateRange dateRange);
    public abstract IDocumentQueryBuilder AddDateRange(DateTime start, DateTime end);
    public abstract DocumentQuery CreateQuery(bool includeItemCount = false);
    public abstract Task<DocumentQuery> CreateQueryAsync(bool includeItemCount = false);

}
public interface IDocumentQueryBuilder
{
    QueryType Type { get; }
    int MaxResults { get; set; }
    IReadOnlyCollection<IOnBaseItemTypeService> Items { get; }
    IReadOnlyCollection<QueryKeyword> QueryKeywords { get; }
    IReadOnlyCollection<DateRange> DocumentDateRanges { get; }
    IReadOnlyCollection<UserDefinedDisplayColumn> UserDisplayColumns { get; }
    IDocumentQueryBuilder AddItem(IOnBaseItemTypeService item);
    IDocumentQueryBuilder AddQueryKeyword(string keywordTypeName, object value, QueryKeywordOperator queryOperator, QueryKeywordRelation queryRelation);
    IDocumentQueryBuilder AddQueryKeyword(long keywordTypeId, object value, QueryKeywordOperator queryOperator, QueryKeywordRelation queryRelation);
    IDocumentQueryBuilder AddDateRange(DateRange dateRange);
    IDocumentQueryBuilder AddDateRange(DateTime start, DateTime end);
    DocumentQuery CreateQuery(bool includeItemCount = false);
    Task<DocumentQuery> CreateQueryAsync(bool includeItemCount = false);
}