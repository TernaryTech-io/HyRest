using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class DocumentTypeGroups : OnBaseItemTypeCollectionService<OnBaseCore, DocumentTypeGroup>
{
    internal DocumentTypeGroups(OnBaseCore core) : base(core)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.GetDocumentTypeGroupCollection(null, null, Options.DefaultLanguage), token);
        if (col != null)
        {
            col.Items
                .Select(i => new DocumentTypeGroup(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
        base.GetCollection(token);
    }    
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
