using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class DocumentTypeGroups : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, DocumentTypeGroup>
{
    internal DocumentTypeGroups(OnBaseCore core) : base(core)
    {

    }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetDocumentTypeGroupCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new DocumentTypeGroup(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }    
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
