using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class DocumentLocks : OnBaseItemCollectionService<IOnBaseDocumentAPI, OnBaseCore, DocumentLock>
{
    private readonly DocumentModel _doc;
    internal DocumentLocks(OnBaseCore core, DocumentModel doc) : base(core)
    {
        _doc = doc;
    }
    public void CreateDocumentLock(LockType lockType)
        => CreateDocumentLockAsync(lockType).Wait();
    public Task CreateDocumentLockAsync(LockType lockType)
        => Module.Run(Api.PostDocumentLocks(_doc.Id, lockType));
    public void DeleteDocumentLock(LockType lockType)
        => DeleteDocumentLockAsync(lockType).Wait();
    public Task DeleteDocumentLockAsync(LockType lockType)
        => Module.Run(Api.DeleteDocumentLock(_doc.Id, lockType));
    protected override async Task GetCollection()
    {
        var coll = await Module.Run(Api.GetDocumentLocks(_doc.Id));
        coll.Items
            .Select(l => new DocumentLock(Module, _doc, l))
            .ToList()
            .ForEach(l => Add(l));
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
