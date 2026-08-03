namespace HyRest.DocumentManagement;

/// <summary>
/// The Document Management API
/// </summary>
public sealed partial class OnBaseCore : OnBaseModule<IOnBaseDocumentAPI>, IOnBaseCore
{    
    public AutoFillKeywordSets AutoFillKeywordSets { get; }
    public CustomQueries CustomQueries { get; set; }
    public DocumentTypeGroups DocumentTypeGroups { get; }
    public DocumentTypes DocumentTypes { get; }
    public FileTypes FileTypes { get; }
    public KeywordTypeGroups KeywordTypeGroups { get; }
    public KeywordTypes KeywordTypes { get; }
    public NoteTypes NoteTypes { get; }
    internal OnBaseCore(IOnBaseApp app) 
        : base(app)
    {       
        AutoFillKeywordSets = new AutoFillKeywordSets(this);
        CustomQueries = new CustomQueries(this);
        DocumentTypeGroups = new DocumentTypeGroups(this);
        DocumentTypes = new DocumentTypes(this);
        FileTypes = new FileTypes(this);
        KeywordTypeGroups = new KeywordTypeGroups(this);
        KeywordTypes = new KeywordTypes(this);
        NoteTypes = new NoteTypes(this);  
    }
    public Task<Document?> GetDocumentByIdAsync(long id)
        => GetDocumentByIdAsync(id.ToString());
    public Document? GetDocumentById(long id)
        => GetDocumentById(id.ToString());
    public Document? GetDocumentById(string id)
    {
        var docTask = GetDocumentByIdAsync(id);
        docTask.Wait();
        if (docTask.IsCompletedSuccessfully)
            return docTask.Result;
        else
            return null;
    }
    public async Task<Document?> GetDocumentByIdAsync(string id)
    {
        var doc = await Run(Api<IOnBaseDocumentAPI>().GetDocumentById(id));
        if (doc != null)
            return new Document(this, doc);
        else
            return null;
    }

    public TQuery CreateDocumentQueryBuilder<TQuery>() where TQuery : class, IDocumentQueryBuilder
    {
        var builder = (TQuery?)Activator.CreateInstance(typeof(TQuery), [this]);
        if (builder != null)
            return builder;
        else throw new Exception($"Could not create a Document Query from type {typeof(TQuery).Name}");
    }
    
    internal static OnBaseCore Create(IOnBaseApp app)
    {        
        return new OnBaseCore(app);
    }
}

