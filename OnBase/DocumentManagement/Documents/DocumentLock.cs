using HyRest.Utilities;
using System.Reflection;

namespace HyRest.DocumentManagement;

public sealed class DocumentLock : OnBaseItemService<OnBaseCore, LockInfoModel>
{
    private readonly DocumentModel _doc;
    internal DocumentLock(OnBaseCore core, DocumentModel doc, LockInfoModel lockInfo) : base(core, lockInfo)
    {
        _doc = doc;
    }
    public void DeleteLock() => DeleteLockAsync().Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task DeleteLockAsync(CancellationToken token = default) => Module.Run(Module.Api.DeleteDocumentLock(_doc.Id, Item.LockType), token);
    public void CreateLock(LockType lockType) => CreateLockAsync(lockType).Wait(Module.App.ClientOptions.RequestTimeOut);
    public Task CreateLockAsync(LockType lockType, CancellationToken token = default)
        => Module.Run(Module.Api.PostDocumentLocks(_doc.Id, lockType), token);
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
