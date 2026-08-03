using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public sealed class DocumentTypes : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, DocumentType>
{
    internal DocumentTypes(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetDocumentTypeCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new DocumentType(Module, i))
                .ToList()
                .ForEach(i => _items.Add(i));
        }
    }   
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
