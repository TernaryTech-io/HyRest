using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class DocumentLocks : OnBaseItemCollectionService<OnBaseCore, DocumentLock>
{
    private readonly DocumentModel _doc;
    internal DocumentLocks(OnBaseCore core, DocumentModel doc) : base(core)
    {
        _doc = doc;
    }
    public void CreateDocumentLock(LockType lockType)
        => CreateDocumentLockAsync(lockType).Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task CreateDocumentLockAsync(LockType lockType, CancellationToken token = default)
        => Module.Run(Module.Api.PostDocumentLocks(_doc.Id, lockType),token);
    public void DeleteDocumentLock(LockType lockType)
        => DeleteDocumentLockAsync(lockType).Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task DeleteDocumentLockAsync(LockType lockType, CancellationToken token = default)
        => Module.Run(Module.Api.DeleteDocumentLock(_doc.Id, lockType), token);
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var coll = await Module.Run(Module.Api.GetDocumentLocks(_doc.Id), token);
        coll.Items
            .Select(l => new DocumentLock(Module, _doc, l))
            .ToList()
            .ForEach(l => Add(l));
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
