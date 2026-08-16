using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class DocumentArchiveProperties : OnBaseRestService, IAsyncDisposable, IDisposable
{
    private OnBaseCore _core => (OnBaseCore)Module;
    private readonly DocumentArchivePropertiesModel _model;     
    private FileType? _fileType { get; set; }
    private DocumentType _documentType { get; set; }
    private List<ArchiveFile> _archiveFiles { get; set; } = [];
    private string? _documentId { get; set; }
    private bool _deleteOnDispose => Files.Any(f => f.Id != null);
    internal DocumentArchiveProperties(OnBaseCore core, DocumentType documentType) : base(core)
    {
        _model = new DocumentArchivePropertiesModel();
        SetDocumentType(documentType);    
        GetKeywordCollectionAsync().Wait(_core.App.RequestTimeOut);
        _model.DocumentDate = DateTime.Now;
    }
    public FileType FileType
    {
        get
        {
            if (_fileType == null && Files.Count > 0)
                GetFileTypeAsync().Wait(_core.App.RequestTimeOut);
            return _fileType;
        }
    }
    public DocumentType DocumentType { get => _documentType = _documentType; set => SetDocumentType(value); }
    public KeywordCollection KeywordCollection { get => new KeywordCollection(_core,_model.KeywordCollection); set => SetKeywordCollection(value); }
    public IReadOnlyCollection<ArchiveFile> Files => _archiveFiles.AsReadOnly();
    public bool StoreAsNew { get => _model.StoreAsNew; set => _model.StoreAsNew = value; }
    public string Comment { get => _model.Comment; set => _model.Comment = value; }
    public DateTime DocumentDate { get => _model.DocumentDate.DateTime; set => _model.DocumentDate = value; }
    
    public Document? ArchiveDocument()
    {
        var task = ArchiveDocumentAsync();
        task.Wait(_core.App.RequestTimeOut);
        if (task.IsCompletedSuccessfully)
            return task.Result;
        return null;
    }
    public async Task<Document?> ArchiveDocumentAsync(CancellationToken token = default)
    {
        if (FileType == null)
            await GetFileTypeAsync(token);
        await StageUploadAsync(token);
        await UploadBytesAsync(token);
        await AddMetadataAsync(token);
        return await _core.GetDocumentByIdAsync(_documentId, token);
    }
    public async ValueTask DisposeAsync()
    {
        if(_deleteOnDispose)
        {
            await Parallel.ForEachAsync(Files, async (file, ct) =>
            {
                await _core.Service.DeleteFileUpload(file.Id);
            });
        }
    }
    public void Dispose()
    {
        if (_deleteOnDispose)
            DisposeAsync();
    }
    internal async Task GetKeywordCollectionAsync(CancellationToken token = default)
    {
        var kc = await _documentType.GetDefaultKeywordsAsync(token);
        if (kc != null)
            _model.KeywordCollection = kc.GetModel();
    }
    internal void SetFileType(FileType ft)
    {
        _model.FileTypeId = ft.Id.ToString();
        _fileType = ft;
    } 
    internal void SetDocumentType(DocumentType dt)
    {
        _documentType = dt;
        _model.DocumentTypeId = dt.Id.ToString();
    }
    internal void SetKeywordCollection(KeywordCollection collection)
    {
        _model.KeywordCollection = collection.GetModel();
    }    
    private async Task StageUploadAsync(CancellationToken token = default)
    {
        while(!token.IsCancellationRequested)
        {
            foreach (var a in Files)
            {
                var reqBody = a.CreateUploadRequest();
                var resp = await _core.Service.PostFileUpLoad(reqBody, token);
                if (resp != null)
                    a.AddUploadPostResponse(resp);
            }
            break;
        }
    }
    private async Task UploadBytesAsync(CancellationToken token = default)
    {
        int successful = 0;
        while(!token.IsCancellationRequested)
        {
            foreach (var a in Files)
            {
                var chunks = a.Bytes.Chunk(a.PartSize);
                int partNo = 0;
                int partSuccess = 0;
                foreach (var chunk in chunks)
                {
                    partNo++;
                    var partSize = chunk.Length;
                    var binaryContent = new ByteArrayContent(chunk);
                    await _core.Service.PutFileUpLoad(a.Id, partNo, binaryContent, token);
                    partSuccess++;
                }
                if (partSuccess == chunks.Count())
                    successful++;
            }
            if (successful != Files.Count)
                throw new Exception("Not all parts where successfully uploaded.");
            break;
        }        
    }    
    private async Task GetFileTypeAsync(CancellationToken token = default)
    {
        if(_fileType == null)
        {
            var file = _archiveFiles[0];
            var fileType = await _core.FileTypes.BestGuessAsync(file.Extension, token) 
                ?? throw new Exception($"A file type couldnot be determined from extension {file.Extension}.");
            fileType = _core.FileTypes.Find(fileType.Id);
            SetFileType(fileType);
        }        
    }
    private async Task AddMetadataAsync(CancellationToken token = default)
    {
        _model.KeywordCollection = KeywordCollection.GetModel();
        _model.Uploads = Files.Select(f => new UploadModel { Id = f.Id }).ToList();
        var resp = await _core.Service.PostDocument(_model, token);
        if (resp != null)
            _documentId = resp.Id;
    }
    public DocumentArchiveProperties WithFileType(FileType fileType)
    {
        _fileType = fileType;
        return this;
    }
    public DocumentArchiveProperties WithStream(Stream stream, string fileExtension)
    {
        var file = new ArchiveFile(stream, fileExtension);
        _archiveFiles.Add(file);
        return this;
    }
    public DocumentArchiveProperties WithBytes(byte[] bytes, string fileExtension)
    {
        var file = new ArchiveFile(bytes, fileExtension);
        _archiveFiles.Add(file);
        return this;
    }
    public DocumentArchiveProperties WithFile(FileInfo fileInfo)
    {
        var file = new ArchiveFile(fileInfo.FullName);
        _archiveFiles.Add(file);
        return this;
    }
    public DocumentArchiveProperties WithFile(string fullPath)
    {
        var file = new ArchiveFile(fullPath);
        _archiveFiles.Add(file);
        return this;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}

public class ArchiveFile
{
    private int _numberOfParts { get; set; }
    private int _partSize { get; set; }
    private byte[] _bytes { get; set; }
    private string _extension { get; set; }
    public ArchiveFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new Exception($"The file at '{filePath}' does not exist or is not accessible");
        _bytes = File.ReadAllBytes(filePath);
        _extension = Path.GetExtension(filePath).Replace(".", "");
    }
    public ArchiveFile(Stream stream, string fileExtension)
    {
        _extension = fileExtension;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            _bytes = memoryStream.ToArray();
        }
    }
    public ArchiveFile(byte[] bytes, string fileExtension)
    {
        _extension = fileExtension;
        _bytes = bytes;
    }
    public string Id { get; set; } = string.Empty;
    public int NumberOfParts => _numberOfParts;
    public int PartSize => _partSize;
    public byte[] Bytes => _bytes;
    public string Extension => _extension;
    public void AddUploadPostResponse(UploadsPostResponseModel resp)
    {
        Id = resp.Id;
        _partSize = resp.FilePartSize;
        _numberOfParts = resp.NumberOfParts;        
    }
    public UploadPostRequestModel CreateUploadRequest()
    {
        return new UploadPostRequestModel
        {
            FileSize = Bytes.Length,
            FileExtension = Extension
        };
    }
}
