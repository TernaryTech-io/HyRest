

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class NoteTypes : OnBaseItemTypeCollectionService<OnBaseCore, NoteType>
{
    internal NoteTypes(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.GetNoteTypeCollection(null, Options.DefaultLanguage), token);
        if (col != null)
        {
            col.Items
                .Select(i => new NoteType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
        base.GetCollection(token);
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}