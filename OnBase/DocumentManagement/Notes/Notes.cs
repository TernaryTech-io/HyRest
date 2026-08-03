

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class Notes : OnBaseItemCollectionService<IOnBaseDocumentAPI, OnBaseCore, Note>
{
    internal Notes(OnBaseCore core) : base(core)
    {

    }
    public async Task<Note?> GetAsync(string id)
    {
        var resp = await Api.GetNoteByNoteId(id);
        if (resp.IsSuccessful)
            return new Note(Module, resp.Content);
        else if (resp.Error != null)
            throw resp.Error;
        else
            return null;
    }
    public async Task<Note?> UpdateAsync(string id, UpdateNoteProperties properties)
    {
        var model = await Module.Run(Api.PatchNoteByNoteId(id, properties));
        if (model != null)
            return new Note(Module, model);
        return null;
    }
    public Task DeleteAsync(string id)
        => Api.DeleteNoteByNoteId(id);
    protected override Task GetCollection() => throw new NotImplementedException();
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
