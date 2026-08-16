

using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class Notes : OnBaseItemCollectionService<OnBaseCore, Note>
{
    internal Notes(OnBaseCore core) : base(core)
    {

    }
    public async Task<Note?> GetAsync(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetNote(id, token);
        if (model != null)
        {
            return new Note(Module, model);
        }
        else
            return null;
    }
    public async Task<Note?> UpdateAsync(string id, UpdateNoteProperties properties, CancellationToken token = default)
    {
        var model = await Module.Service.PatchNote(id, properties,token);
        if (model != null)
            return new Note(Module, model);
        return null;
    }
    public Task DeleteAsync(string id, CancellationToken token = default)
        => Module.Service.DeleteNote(id, token);
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
