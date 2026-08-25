namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<DocumentTypeCollectionModel?> _getDocumentTypes(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetDocumentTypeCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<DocumentTypeModel?> _getDocumentType(string id, CancellationToken token = default)
        => Run(Api.GetDocumentTypeById(id, Options.DefaultLanguage), token);
    public async Task<DocumentTypeCollectionModel?> GetDocumentTypes(CancellationToken token = default)
    {
        var col = await _getDocumentTypes([], [], token);
        if (col != null)
        {
            col.Items.ToList().ForEach(async i =>
            {
                await Cache.SetAsync(i, token, CachePrefix);
            });
            return col;
        }
        else
            return null;
    }
    public async Task<DocumentTypeModel?> GetDocumentType(string identifier, CancellationToken token = default)
    {
        DocumentTypeModel? item = null;
        if (Cache.TryGetValue(identifier, out item, CachePrefix))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getDocumentType(identifier, token);
        else
        {
            var col = await _getDocumentTypes([], [identifier], token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item, token, CachePrefix);
        return item;
    }
    public Task<KeywordTypeGroupCollectionModel?> GetKeywordTypeGroupsForDocumentType(string id, CancellationToken token = default)
        => Run(Api.GetKeywordTypeGroupCollectionForDocumentType(id, Options.DefaultLanguage), token);
    public Task<KeywordCollectionModel?> GetDefaultKeywordsForDocumentType(string id, CancellationToken token)
        => Run(Api.GetDefaultKeywordCollectionForDocumentType(id, Options.DefaultLanguage), token);
}