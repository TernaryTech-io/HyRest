using HyRest.OnBase.Core;
using Refit;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    public Task<DocumentModel?> GetDocumentById(string id, CancellationToken token)
        => Run(Api.GetDocumentById(id), token);
    public Task DeleteDocument(string id, CancellationToken token = default)
        => Run(Api.DeleteDocumentById(id),token);
    public Task PatchDocumentDate(string id, DocumentPatchRequestModel documentDate, CancellationToken token = default)
        => Run(Api.PatchDocumentById(id, documentDate), token);
    public Task PutKeywordsForDocument(string id, KeywordCollectionModel keyColl, CancellationToken token = default)
        => Run(Api.PutKeywordCollectionForDocument(id, keyColl), token);
    public Task<KeywordCollectionModel?> GetKeywordsForDocument(string id, bool? unmask = false, CancellationToken token = default)
        => Run(Api.GetKeywordCollectionForDocument(id, unmask), token);
    public Task<RevisionCollectionModel?> GetDocumentRevisions(string id, CancellationToken token = default)
        => Run(Api.GetRevisionCollectionForDocument(id), token);
    public Task<RevisionModel?> GetDocumentRevision(string id, string revisionId, CancellationToken token = default)
        => Run(Api.GetRevisionByIdForDocument(id, revisionId), token);
    public Task<NoteCollectionModel?> GetNotesForDocument(string id, string revisionid = "latest", CancellationToken token = default)
        => Run(Api.GetNoteCollectionForDocument(id, revisionid), token);
    public Task<DocumentHistory?> GetDocumentHistory(string id, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null, string? userId = null, CancellationToken token = default)
        => Run(Api.History(id,startDate,endDate,userId), token);
    public Task<LockInfoCollectionModel?> GetDocumentLocks(string id, CancellationToken token = default)
        => Run(Api.GetDocumentLocks(id), token);
    public Task CreateDocumentLock(string id, LockType type, CancellationToken token = default)
        => Run(Api.PostDocumentLocks(id, type), token);
    public Task DeleteDocumentLock(string id, LockType type, CancellationToken token = default)
        => Run(Api.DeleteDocumentLock(id, type), token);
    public Task<ApiResponse<Stream>> GetDocumentContent(string id, string revisionId = "latest", string fileTypeId = "default", string? pages = null, Context? context = Context.View,
        int? height = null, int? width = null, Fit? fit = null, string? accept = "*/*", string? if_Match = null, string? range = null, CancellationToken token = default)
        => Api.GetContentForRenditionOfRevisionOfDocument(id, revisionId, fileTypeId, pages, context, height, width, fit, accept,if_Match, range);
    public Task PostNoteOnDocument(string id, AddNoteProperties addNoteProperties, string revisionId = "latest", CancellationToken token = default)
        => Run(Api.PostNoteOnDocument(id,revisionId, addNoteProperties), token);
    public Task DeleteFileUpload(string id, CancellationToken token = default)
        => Run(Api.DeleteFileUploadById(id), token);
    public Task<UploadsPostResponseModel?> PostFileUpLoad(UploadPostRequestModel model, CancellationToken token = default)
        => Run(Api.PostFileUploadMetadata(model), token);
    public Task PutFileUpLoad(string id, int partNo, ByteArrayContent content, CancellationToken token = default)
        => Run(Api.PutFileUploadById(id, partNo, content), token);
    public Task<DocumentsPostResponse?> PostDocument(DocumentArchivePropertiesModel props, CancellationToken token = default)
        => Run(Api.PostDocument(props), token);
    public Task<RenditionCollectionModel?> GetRevisionRenditions(string id, string revisionId, CancellationToken token = default)
        => Run(Api.GetRenditionCollectionForRevisionOfDocument(id, revisionId), token);
}