namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<KeywordTypeCollectionModel?> _getKeywordTypes(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetKeywordTypeCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<KeywordTypeModel?> _getKeywordType(string id, CancellationToken token = default)
        => Run(Api.GetKeywordTypeById(id, Options.DefaultLanguage), token);
    public async Task<KeywordTypeCollectionModel?> GetKeywordTypes(CancellationToken token = default)
    {
        var col = await _getKeywordTypes([], [], token);
        if (col != null)
        {
            col.Items.ToList().ForEach(async i =>
            {
                await Cache.SetAsync(i, token);
            });
            return col;
        }
        else
            return null;
    }
    public async Task<KeywordTypeModel?> GetKeywordType(string identifier, CancellationToken token = default)
    {
        KeywordTypeModel? item = null;
        if (Cache.TryGetValue(identifier, out item))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getKeywordType(identifier, token);
        else
        {
            var col = await _getKeywordTypes([], [identifier], token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item);
        return item;
    }
}