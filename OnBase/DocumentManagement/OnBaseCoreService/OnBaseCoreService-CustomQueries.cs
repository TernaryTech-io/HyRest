namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<CustomQueryCollectionModel?> _getCustomQueries(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetCustomQueryCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<CustomQueryModel?> _getCustomQuery(string id, CancellationToken token = default)
        => Run(Api.GetCustomQueryById(id, Options.DefaultLanguage), token);
    public async Task<CustomQueryCollectionModel?> GetCustomQueries(CancellationToken token = default)
    {
        var col = await _getCustomQueries([], [], token);
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
    public async Task<CustomQueryModel?> GetCustomQuery(string identifier, CancellationToken token = default)
    {
        CustomQueryModel? item = null;
        if (Cache.TryGetValue(identifier, out item))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getCustomQuery(identifier, token);
        else
        {
            var col = await _getCustomQueries([], [identifier], token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item, token, CachePrefix);
        return item;
    }
    public Task<CustomQueryKeywordTypeCollectionModel?> GetKeywordsForCustomQuery(string id, CancellationToken token = default)
        => Run(Api.GetKeywordTypeCollectionForCustomQuery(id), token);
    public Task<QueryResultsModel?> GetQueryResults(string id, CancellationToken token = default)
        => Run(Api.GetResultCollectionForDocumentQuery(id), token);
    public Task<QueriesPostResponseModel?> PostDocumentQuery(QueryInformationModel model, bool includeItemCount = false, CancellationToken token = default)
        => Run(Api.PostDocumentQuery(model, includeItemCount), token);
}