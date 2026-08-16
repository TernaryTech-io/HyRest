using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class NoteTypes : OnBaseItemTypeCollectionService<OnBaseCore, NoteType>
{
    internal NoteTypes(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetNoteTypes(token);
        col?.Items
                .Select(i => new NoteType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<NoteType?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetNoteType(id, token);
        if (model != null)
            return new NoteType(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}