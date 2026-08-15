using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public sealed class DocumentLocks : OnBaseBaseCollectionService<OnBaseCore, DocumentLock>
{
    private readonly DocumentModel _doc;
    internal DocumentLocks(OnBaseCore core, DocumentModel doc) : base(core)
    {
        _doc = doc;
    }
    public void CreateDocumentLock(LockType lockType)
        => CreateDocumentLockAsync(lockType).Wait(Module.App.RequestTimeOut);
    public Task CreateDocumentLockAsync(LockType lockType, CancellationToken token = default)
        => Module.Service.CreateDocumentLock(_doc.Id, lockType, token);
    public void DeleteDocumentLock(LockType lockType)
        => DeleteDocumentLockAsync(lockType).Wait(Module.App.RequestTimeOut);
    public Task DeleteDocumentLockAsync(LockType lockType, CancellationToken token = default)
        => Module.Service.DeleteDocumentLock(_doc.Id, lockType, token);
    public async Task GetLocks(CancellationToken token = default)
    {
        var col = await Module.Service.GetDocumentLocks(_doc.Id.ToString(), token);
        col?.Items
            .Select(l => new DocumentLock(Module, _doc, l))
            .ToList()
            .ForEach(l => Add(l));
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
