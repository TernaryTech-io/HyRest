using System.Reflection;
using static Duende.AccessTokenManagement.AccessTokenRequestHandler;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{    
    private Task<AutoFillKeywordSetCollectionModel?> _getAutoFillSets(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetAutofillKeywordSetCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<AutoFillKeywordSetModel?> _getAutoFillSet(string id, CancellationToken token = default)
        => Run(Api.GetAutofillKeywordSetById(id, Options.DefaultLanguage), token);
    public async Task<AutoFillKeywordSetCollectionModel?> GetAutoFillKeywordSets(CancellationToken token = default)
    {
        var col = await _getAutoFillSets([], [], token);
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
    public async Task<AutoFillKeywordSetModel?> GetAutoFillKeywordSet(string identifier, CancellationToken token = default)
    {
        AutoFillKeywordSetModel? item = null;
        if(Cache.TryGetValue(identifier, out item))
            return item;

        if (long.TryParse(identifier, out long id))
            item =  await _getAutoFillSet(identifier,token);
        else
        {
            var col = await _getAutoFillSets([], [identifier], token);
            if(col != null)
                item = col.Items.FirstOrDefault();
        }
        if(item != null)
            await Cache.SetAsync(item, token, CachePrefix);
        return item;
    }
    public Task<AutoFillKeywordSetKeywordTypeCollectionModel?> GetAutoFillKeywordSetKeywordTypes(string id, CancellationToken token = default)
        => Run(Api.GetKeywordTypeCollectionForAutofillKeywordSet(id), token);
    public Task<KeywordSetDataCollectionModel?> GetAutoFillKeySetData(string id, string primaryValue, CancellationToken token = default)
        => Run(Api.GetKeywordDataCollectionForAutofillKeywordSet(id, primaryValue, Options.DefaultLanguage), token);
}