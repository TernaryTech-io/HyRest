namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<FileTypeCollectionModel?> _getFileTypes(IEnumerable<string> ids, IEnumerable<string> systemNames, CancellationToken token = default)
        => Run(Api.GetFileTypeCollection(ids, systemNames, Options.DefaultLanguage), token);
    private Task<FileTypeModel?> _getFileType(string id, CancellationToken token = default)
        => Run(Api.GetFileTypeById(id, Options.DefaultLanguage), token);
    public async Task<FileTypeCollectionModel?> GetFileTypes(CancellationToken token = default)
    {
        var col = await _getFileTypes([], [], token);
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
    public async Task<FileTypeModel?> GetFileType(string identifier, CancellationToken token = default)
    {
        FileTypeModel? item = null;
        if (Cache.TryGetValue(identifier, out item, CachePrefix))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getFileType(identifier, token);
        else
        {
            var col = await _getFileTypes([], [identifier], token);
            if (col != null)
                item = col.Items.FirstOrDefault();
        }
        if (item != null)
            await Cache.SetAsync(item, token, CachePrefix);
        return item;
    }
    public Task<FileTypeModel> GetBestGuessFileType(string extension, CancellationToken token)
        => Run(Api.GetFileTypeForUpload(extension, Options.DefaultLanguage), token);
}