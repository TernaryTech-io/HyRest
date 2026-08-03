using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public sealed class FileTypes : OnBaseItemTypeCollectionService<IOnBaseDocumentAPI, OnBaseCore, FileType>
{
    internal FileTypes(OnBaseCore core) : base(core)
    {
        
    }
    public FileType? BestGuess(string extension)
    {
        var task = BestGuessAsync(extension);
        task.Wait();
        if (task.IsCompletedSuccessfully)
            return Module.FileTypes.Find(task.Result.Id);
        else
            return null;
    }
    public async Task<FileType?> BestGuessAsync(string extension)
    {
        var model = await Module.Run(Api.GetFileTypeForUpload(extension, Options.DefaultLanguage));
        if (model != null)
            return new FileType(Module, model);
        return null;
    }
    protected override async Task GetCollection()
    {
        var col = await Module.Run(Api.GetFileTypeCollection(null, null, Options.DefaultLanguage));
        if (col != null)
        {
            col.Items
                .Select(i => new FileType(Module, i))
                .ToList()
                .ForEach(i => Add(i));
        }
    }    
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
