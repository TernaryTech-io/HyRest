using System.Reflection;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<KeywordTypeGroupCollectionModel?> _getKeywordTypeGroups(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetKeywordTypeGroupCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<KeywordTypeGroupModel?> _getKeywordTypeGroup(string id, CancellationToken token = default)
        => Run(Api.GetKeywordTypeGroupById(id, Options.DefaultLanguage), token);
    public async Task<KeywordTypeGroupCollectionModel?> GetKeywordTypeGroups(CancellationToken token = default)
    {
        var col = await _getKeywordTypeGroups([], [], token);
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
    public async Task<KeywordTypeGroupModel?> GetKeywordTypeGroup(string identifier, CancellationToken token = default)
    {
        KeywordTypeGroupModel? item = null;
        if (Cache.TryGetValue(identifier, out item, CachePrefix))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getKeywordTypeGroup(identifier, token);
        else
        {
            var col = await _getKeywordTypeGroups([], [identifier], token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item, token, CachePrefix);
        return item;
    }
    public Task<KeywordTypeCollectionModel?> GetKeywordTypesForKeywordTypeGroup(string id, CancellationToken token = default)
        => Run(Api.GetKeywordTypeCollectionForKeywordTypeGroup(id), token);
}