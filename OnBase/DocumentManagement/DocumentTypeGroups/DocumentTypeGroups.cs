using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public sealed class DocumentTypeGroups : OnBaseItemTypeCollectionService<OnBaseCore, DocumentTypeGroup>
{
    internal DocumentTypeGroups(OnBaseCore core) : base(core)
    {

    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetDocumentTypeGroups(token);
        col?.Items
                .Select(i => new DocumentTypeGroup(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<DocumentTypeGroup?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetDocumentTypeGroup(id, token);
        if (model != null)
            return new DocumentTypeGroup(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
