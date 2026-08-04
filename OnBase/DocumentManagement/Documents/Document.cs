using System.Text.Json.Serialization;
using Ternary.DataConversions.Extensions;
using HyRest.Utilities;
using HyRest.Administration;

namespace HyRest.DocumentManagement;
public sealed class Document : OnBaseItemService<IOnBaseDocumentAPI, OnBaseCore, DocumentModel>
{
    private DocumentType? _documentType { get; set; }
    private DocumentLocks? _locks { get; set; }
    private List<Revision> _revisions { get; set; }
    private List<Note> _notes { get; set; }
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
            if (_documentType == null)
                GetDocumentType();
            return _documentType;
        }
    }
    [JsonIgnore]
    public IReadOnlyCollection<Note> Notes
    {
        get
        {
            if (_notes == null)
                GetNoteCollection().Wait();
            return _notes ?? [];
        }
    }
    public User CreatedBy
    {
        get
        {
            if (_createdBy == null)
                GetUserInfo().Wait();
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
                GetKeywordCollection().Wait();
            return _keywordCollection;
        }
    }
    [JsonIgnore]
    public IReadOnlyCollection<Revision> Revisions
    {
        get
        {
            if (_revisions == null)
                GetRevisions().Wait();
            return _revisions ?? [];
        }
    }
    public DocumentHistory GetHistory(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? userId = null)
    {
        var task = GetHistoryAsync(startDate, endDate, userId);
        task.Wait();
        if (task.IsCompletedSuccessfully)
            return task.Result;
        else
            throw task.Exception?.InnerException ?? task.Exception ?? new Exception("Could not retrieve the document history");
    }
    public Task<DocumentHistory> GetHistoryAsync(DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? userId = null)
        => Module.Run(Api.History(Item.Id, startDate, endDate, userId));
    public Task<NoteCollectionModel> GetNotesForRevision(string revisionId, int? page)
        => Module.Run(Api.GetNoteCollectionForDocument(Item.Id, revisionId, page));

    public FileResponse GetContent(string revisionId = "latest", string fileTypeId = "default", string? pages = null, Context? context = Context.View,
        int? height = null, int? width = null, Fit? fit = null, string? accept = "*/*", string? if_Match = null, string? range = null)
    {
        var task = GetContentAsync(revisionId, fileTypeId, pages, context, height, width, fit, accept, if_Match, range);
        task.Wait();
        if (task.IsCompletedSuccessfully)
            return task.Result;
        else
            throw task.Exception?.InnerException ?? task.Exception ?? new Exception("Failed to retrieve document content.");
    }
    public async Task<FileResponse> GetContentAsync(string revisionId="latest", string fileTypeId ="default", string? pages = null, Context? context = Context.View,
        int? height = null, int? width = null, Fit? fit = null, string? accept = "*/*", string? if_Match = null, string? range = null)
    {
        var response = await Api.GetContentForRenditionOfRevisionOfDocument(Item.Id, revisionId, fileTypeId, pages, context, height, width, fit, accept, if_Match, range);
        return new FileResponse(response);
    }
    public Task Delete() => Api.DeleteDocumentById(Item.Id);
    public Task AddNewNote(AddNoteProperties addNoteProperties, string revisionId = "latest")
        => Module.Run(Api.PostNoteOnDocument(Item.Id, revisionId, addNoteProperties));

    public void UpdateKeywords()
        => UpdateKeywordsAsync().Wait();
    public async Task UpdateKeywordsAsync()
    {
        await Module.Run(Api.PutKeywordCollectionForDocument(Id.ToString(), KeywordCollection.GetModel()));
        await GetKeywordCollection();
    }

    public DocumentReindexProperties CreateDocumentReindexProperties()
        => new DocumentReindexProperties(Module, this);
    public Task UpdateDocDate(DateTime documentDate) => Api.PatchDocumentById(Item.Id, new DocumentPatchRequestModel { DocumentDate = documentDate});
    public async Task<Document> Reindex(DocumentReindexProperties props)
    {
        var resp = await Module.Run(Api.PutDocumentById(Item.Id, props.GetModel()));
        
        return await Module.GetDocumentByIdAsync(Id);
    }
    private void GetDocumentType()
    {
        if(Item.TypeId != null)
        {
            var dt = Module.DocumentTypes.Find(Item.TypeId);
            if (dt != null && dt is DocumentType d)
                _documentType = d;
        }
    }
    private async Task GetRevisions()
    {
        var revCol = await Module.Run(Api.GetRevisionCollectionForDocument(Item.Id));
        if (revCol != null && revCol.Items.Count > 0)
            _revisions = revCol.Items.Select(i => new Revision(Module, Item, i)).ToList();
    }

    private async Task GetKeywordCollection()
    {
        var keycol = await Module.Run(Api.GetKeywordCollectionForDocument(Item.Id, false));
        if (keycol != null)
            _keywordCollection = new KeywordCollection(Module, keycol);
    }
    private async Task GetNoteCollection(string revisionId = "latest")
    {
        var noteCol = await Module.Run(Api.GetNoteCollectionForDocument(Item.Id, revisionId));
        if(noteCol != null)
            _notes = noteCol.Items                    
                    .Select(c => new Note(Module, c))
                    .ToList();
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
    private async Task GetUserInfo()
    {
        var admin = (OnBaseAdministration)Module.App.Administration;
        _createdBy = admin.Users.Find(Item.CreatedByUserId);
    }
}
