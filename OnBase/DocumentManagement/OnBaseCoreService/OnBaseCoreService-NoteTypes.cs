namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    private Task<NoteTypeCollectionModel?> _getNoteTypes(IEnumerable<string> ids, CancellationToken token = default)
        => Run(Api.GetNoteTypeCollection(ids,  Options.DefaultLanguage), token);
    private Task<NoteTypeModel?> _getNoteType(string id, CancellationToken token = default)
        => Run(Api.GetNoteTypeById(id, Options.DefaultLanguage), token);
    public async Task<NoteTypeCollectionModel?> GetNoteTypes(CancellationToken token = default)
    {
        var col = await _getNoteTypes([], token);
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
    public async Task<NoteTypeModel?> GetNoteType(string identifier, CancellationToken token = default)
    {
        NoteTypeModel? item = null;
        if (Cache.TryGetValue(identifier, out item))
            return item;

        if (long.TryParse(identifier, out long id))
            item = await _getNoteType(identifier, token);      
        if (item != null)
            await Cache.SetAsync(item);
        return item;
    }
}