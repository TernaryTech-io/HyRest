using System.Text.Json.Serialization;
using Ternary.DataConversions.Extensions;
using HyRest.Utilities;

namespace HyRest.OnBase.Core;
public sealed class Document : OnBaseItemService<OnBaseCore, DocumentModel>
{
    private DocumentType? _documentType { get; set; }
    private DocumentLocks? _locks { get; set; }
    private List<Revision> _revisions { get; set; }
    private KeywordCollection? _keywordCollection { get; set; }
    private User? _createdBy { get; set; }
    internal Document(OnBaseCore core, DocumentModel doc) : base(core, doc){}
    [JsonPropertyName("name")]
    public override string Name => Item.Name ?? string.Empty;

    public DocumentLocks? Locks
    {
        get
        {
            if (_locks == null)
                _locks = new DocumentLocks(Module, Item);
            return _locks;
        }
    }    
    public DocumentType? DocumentType
    {
        get
        {
            if (_documentType == null && Item.TypeId != null)
                _documentType = Module.DocumentTypes[Item.TypeId];
            return _documentType;
        }
    }
    public User CreatedBy
    {
        get
        {
            if (_createdBy == null)
                GetUserInfo();
            return _createdBy;
        }
    }    
    public DateTime StoredDate { get => Item.StoredDate.ConvertTo<DateTime>(); }
    public DateTime DocumentDate { get => Item.DocumentDate.DateTime;  }
    public DocumentStatus Status => Item.Status;
    public CaptureProperties? CaptureProperties => Item.CaptureProperties;
    [JsonIgnore]
    public KeywordCollection KeywordCollection
    {
        get
        {
            if (_keywordCollection == null)
                GetKeywordCollection().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _keywordCollection;
        }
    }
    [JsonIgnore]
    public IReadOnlyList<Revision> Revisions
    {
        get
        {
            if (_revisions == null)
                GetRevisions().Wait(Module.App.ClientOptions.RequestTimeOut);
            return _revisions ?? [];
        }
    }
    public DocumentHistory GetHistory(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? userId = null)
    {
        var task = GetHistoryAsync(startDate, endDate, userId);
        if (task.Wait(Module.App.ClientOptions.RequestTimeOut) && task.IsCompletedSuccessfully)
            return task.Result;
        else
            return new();
    }
    public Task<DocumentHistory?> GetHistoryAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? userId = null, CancellationToken token = default)
        => Module.Service.GetDocumentHistory(Item.Id, startDate, endDate, userId, token);

    public IReadOnlyList<Note> GetNotesForRevision(string revisionId = "latest", int? page = null)
    {
        var task = GetNotesForRevisionAsync(revisionId, page);
        if(task.Wait(Module.App.ClientOptions.RequestTimeOut) && task.IsCompletedSuccessfully)
        {
            return task.Result;
        }
        return [];
    }
    public async Task<IReadOnlyList<Note>> GetNotesForRevisionAsync(string revisionId = "latest", int? page = null, CancellationToken token = default)
    {
        List<Note> notes = [];
        var col = await Module.Service.GetNotesForDocument(Item.Id, revisionId, token);
        col?.Items.ToList().ForEach(n => notes.Add(new Note(Module, n)));
        return notes;
    }

    public FileResponse GetContent(string revisionId = "latest", string fileTypeId = "default", string? pages = null, Context? context = Context.View,
        int? height = null, int? width = null, Fit? fit = null, string? accept = "*/*", string? if_Match = null, string? range = null)
    {
        var task = GetContentAsync(revisionId, fileTypeId, pages, context, height, width, fit, accept, if_Match, range);
        
        if (task.Wait(Module.App.ClientOptions.RequestTimeOut) && task.IsCompletedSuccessfully)
            return task.Result;
        else
            throw task.Exception?.InnerException ?? task.Exception ?? new Exception("Failed to retrieve document content.");
    }
    public async Task<FileResponse> GetContentAsync(string revisionId="latest", string fileTypeId ="default", string? pages = null, Context? context = Context.View,
        int? height = null, int? width = null, Fit? fit = null, string? accept = "*/*", string? if_Match = null, string? range = null, CancellationToken token = default)
    {
        var response = await Module.Service.GetDocumentContent(Item.Id, revisionId, fileTypeId, pages, context, height, width, fit, accept, if_Match, range, token);
        return new FileResponse(response);
    }
    public void Delete() => DeleteAsync().Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task DeleteAsync(CancellationToken token = default) => Module.Service.DeleteDocument(Item.Id, default);
    public void AddNote(AddNoteProperties addNoteProperties, string revisionId = "latest")
        => AddNoteAsync(addNoteProperties, revisionId).Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task AddNoteAsync(AddNoteProperties addNoteProperties, string revisionId = "latest", CancellationToken token = default)
        => Module.Service.PostNoteOnDocument(Item.Id, addNoteProperties, revisionId, token);
    public void UpdateKeywords()
        => UpdateKeywordsAsync().Wait(Module.App.ClientOptions.RequestTimeOut);
    public async Task UpdateKeywordsAsync(CancellationToken token = default)
    {
        await Module.Service.PutKeywordsForDocument(Id.ToString(), KeywordCollection.GetModel(), token);
        await GetKeywordCollection();
    }
    public DocumentReindexProperties CreateDocumentReindexProperties()
        => new DocumentReindexProperties(Module, this);
    public void UpdateDocumentDate(DateTime documentDate) => UpdateDocumentDateAsync(documentDate).Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task UpdateDocumentDateAsync(DateTime documentDate, CancellationToken token = default) 
        => Module.Service.PatchDocumentDate(Item.Id, new DocumentPatchRequestModel { DocumentDate = documentDate},token);
    //public async Task<Document> ReindexAsync(DocumentReindexProperties props)
    //{
    //    var resp = await Module.Run(Module.Api.PutDocumentById(Item.Id, props.GetModel()));

    //    return await Module.GetDocumentByIdAsync(Id);
    //}
    public async Task<Revision?> GetRevisionAsync(string revisionId, CancellationToken token = default)
    {
        var model = await Module.Service.GetDocumentRevision(Item.Id, revisionId, token);
        if (model != null)
            return new Revision(Module, Item, model);
        else return null;
    }
    private async Task GetRevisions(CancellationToken token = default)
    {
        var revCol = await Module.Service.GetDocumentRevisions(Item.Id,token);
        if (revCol != null && revCol.Items.Count > 0)
            _revisions = revCol.Items.Select(i => new Revision(Module, Item, i)).ToList();
    }

    private async Task GetKeywordCollection(CancellationToken token = default)
    {
        var keycol = await Module.Service.GetKeywordsForDocument(Item.Id, false, token);
        if (keycol != null)
            _keywordCollection = new KeywordCollection(Module, keycol);
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
    private void GetUserInfo()
    {
        var admin = (OnBaseAdministration)Module.App.Administration;
        if(Item.CreatedByUserId != null)
            _createdBy = admin.Users.Find(Item.CreatedByUserId);
    }
}
