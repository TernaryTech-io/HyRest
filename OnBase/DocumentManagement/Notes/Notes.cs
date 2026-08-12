

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class Notes : OnBaseItemCollectionService<OnBaseCore, Note>
{
    internal Notes(OnBaseCore core) : base(core)
    {

    }
    public async Task<Note?> GetAsync(string id, CancellationToken token = default)
    {
        var model = await Module.Run(Module.Api.GetNoteByNoteId(id), token);
        if (model != null)
            return new Note(Module, model);
        else
            return null;
    }
    public async Task<Note?> UpdateAsync(string id, UpdateNoteProperties properties, CancellationToken token = default)
    {
        var model = await Module.Run(Module.Api.PatchNoteByNoteId(id, properties), token);
        if (model != null)
            return new Note(Module, model);
        return null;
    }
    public Task DeleteAsync(string id, CancellationToken token = default)
        => Module.Run(Module.Api.DeleteNoteByNoteId(id),token);
    protected override Task GetCollection(CancellationToken token = default) => throw new NotImplementedException();
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
