

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class NoteTypes : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, NoteType>
{
    internal NoteTypes(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetNoteTypeCollection(null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new NoteType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}