namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<DocumentTypeGroupCollectionModel?> _getDocumentTypeGroups(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetDocumentTypeGroupCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<DocumentTypeGroupModel?> _getDocumentTypeGroup(string id, CancellationToken token = default)
        => Run(Api.GetDocumentTypeGroupById(id, Options.DefaultLanguage), token);
    public async Task<DocumentTypeGroupCollectionModel?> GetDocumentTypeGroups(CancellationToken token = default)
    {
        var col = await _getDocumentTypeGroups([], [], token);
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
    public async Task<DocumentTypeGroupModel?> GetDocumentTypeGroup(string identifier, CancellationToken token = default)
    {
        DocumentTypeGroupModel? item = null;
        if (Cache.TryGetValue(identifier, out item))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getDocumentTypeGroup(identifier, token);
        else
        {
            var col = await _getDocumentTypeGroups([], [identifier], token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item, token, CachePrefix);
        return item;
    }
    public Task<DocumentTypeCollectionModel?> GetDocumentTypesForDocumentTypeGroup(string id, CancellationToken token = default)
        => Run(Api.GetDocumentTypeCollectionForDocumentTypeGroup(id), token);
}