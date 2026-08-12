using HyRest.Utilities;

namespace HyRest.DocumentManagement;
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
        var model = await Module.Run(Module.Api.GetFileTypeForUpload(extension, Options.DefaultLanguage),token);
        if (model != null)
            return new FileType(Module, model);
        return null;
    }
    protected override async Task GetCollection(CancellationToken token)
    {
        var col = await Module.Run(Module.Api.GetFileTypeCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new FileType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
        base.GetCollection(token);
    }    
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
