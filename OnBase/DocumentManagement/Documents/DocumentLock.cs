using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class DocumentLock : OnBaseItemService<IOnBaseDocumentAPI, OnBaseCore, LockInfoModel>
{
    private readonly DocumentModel _doc;
    internal DocumentLock(OnBaseCore core, DocumentModel doc, LockInfoModel lockInfo) : base(core, lockInfo)
    {
        _doc = doc;
    }
    public void DeleteLock() => DeleteLockAsync().Wait();
    public Task DeleteLockAsync() => Module.Run(Api.DeleteDocumentLock(_doc.Id, Item.LockType));
    public void CreateLock(LockType lockType) => CreateLockAsync(lockType).Wait();
    public Task CreateLockAsync(LockType lockType)
        => Module.Run(Api.PostDocumentLocks(_doc.Id, lockType));
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
