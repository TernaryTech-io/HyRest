using HyRest.Utilities;

namespace HyRest.OnBase.Core;
public sealed class FileTypes : OnBaseItemTypeCollectionService<OnBaseCore, FileType>
{
    internal FileTypes(OnBaseCore core) : base(core)
    {
        
    }
    public FileType? BestGuess(string extension)
    {
        var task = BestGuessAsync(extension);
        task.Wait(Module.App.ClientOptions.RequestTimeOut);
        if (task.IsCompletedSuccessfully)
            return Module.FileTypes.Find(task.Result.Id);
        else
            return null;
    }
    public async Task<FileType?> BestGuessAsync(string extension, CancellationToken token = default)
    {
        var model = await Module.Service.GetBestGuessFileType(extension, token);
        if (model != null)
            return Module.FileTypes[model.Id];
        return null;
    }
    protected override async Task GetCollection(CancellationToken token = default)
    {
        var col = await Module.Service.GetFileTypes(token);
        col?.Items
                .Select(i => new FileType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
    }
    protected override async Task<FileType?> GetOne(string id, CancellationToken token = default)
    {
        var model = await Module.Service.GetFileType(id, token);
        if (model != null)
            return new FileType(Module, model);
        return null;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
