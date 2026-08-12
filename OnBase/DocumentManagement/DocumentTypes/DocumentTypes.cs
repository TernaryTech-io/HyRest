using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public sealed class DocumentTypes : OnBaseItemTypeCollectionService<OnBaseCore, DocumentType>
{
    internal DocumentTypes(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Run(Module.Api.GetDocumentTypeCollection(null, null, Options.DefaultLanguage), token);
        if (col != null)
        {
            col.Items
                .Select(i => new DocumentType(Module, i))
                .ToList()
                .ForEach(i => _items.Add(i));
        }
        base.GetCollection(token);
    }   
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
