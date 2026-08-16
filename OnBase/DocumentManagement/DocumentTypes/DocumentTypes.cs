using HyRest.Utilities;

namespace HyRest.OnBase.Core;
public sealed class DocumentTypes : OnBaseItemTypeCollectionService<OnBaseCore, DocumentType>
{
    internal DocumentTypes(OnBaseCore core) : base(core)
    {
        
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetDocumentTypes(token);
        col?.Items
                .Select(i => new DocumentType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<DocumentType?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetDocumentType(id, token);
        if (model != null)
            return new DocumentType(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
