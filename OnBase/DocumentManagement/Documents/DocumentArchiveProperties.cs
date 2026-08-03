using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class DocumentArchiveProperties : OnBaseRestService<IOnBaseDocumentAPI>, IAsyncDisposable, IDisposable
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
        GetKeywordCollection().Wait();
        _model.DocumentDate = DateTime.Now;
    }
    public FileType FileType
    {
        get
        {
            if (_fileType == null && Files.Count > 0)
                GetFileTypeId().Wait();
            return _fileType;
        }
    }
    public DocumentType DocumentType { get => _documentType = _documentType; set => SetDocumentType(value); }
    public KeywordCollection KeywordCollection { get => new KeywordCollection(_core,_model.KeywordCollection); set => SetKeywordCollection(value); }
    public IReadOnlyCollection<ArchiveFile> Files => _archiveFiles.AsReadOnly();
    public bool StoreAsNew { get => _model.StoreAsNew; set => _model.StoreAsNew = value; }
    public string Comment { get => _model.Comment; set => _model.Comment = value; }
    public DateTime DocumentDate { get => _model.DocumentDate.DateTime; set => _model.DocumentDate = value; }
    
    public async Task<Document?> ArchiveDocument()
    {
        if (FileType == null)
            await GetFileTypeId();
        await StageUpload();
        await UploadBytes();
        await AddMetadata();
        return await _core.GetDocumentByIdAsync(_documentId);
    }
    public async ValueTask DisposeAsync()
    {
        if(_deleteOnDispose)
        {
            await Parallel.ForEachAsync(Files, async (file, ct) =>
            {
                await Module.Run(Api.DeleteFileUploadById(file.Id));
            });
        }
    }
    public void Dispose()
    {
        if (_deleteOnDispose)
        {
            Parallel.ForEach(Files, (file, ct) =>
            {
                Module.Run(Api.DeleteFileUploadById(file.Id)).Wait();
            });
        }
    }
    internal async Task GetKeywordCollection()
    {
        var kc = await _documentType.GetDefaultKeywords();
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
    private async Task StageUpload()
    {
        foreach(var a in Files)
        {
            var reqBody = a.CreateUploadRequest();
            var resp = await Module.Run(Api.PostFileUploadMetadata(reqBody));
            if(resp!= null)
                a.AddUploadPostResponse(resp);
        }
    }
    private async Task UploadBytes()
    {
        int successful = 0;
        foreach(var a in Files)
        {
            var chunks = a.Bytes.Chunk(a.PartSize);
            int partNo = 0;
            int partSuccess = 0;
            foreach (var chunk in chunks)
            {
                partNo++;
                var partSize = chunk.Length;
                var binaryContent = new ByteArrayContent(chunk);
                await Module.Run(Api.PutFileUploadById(a.Id, partNo, binaryContent));
                partSuccess++;
            }
            if (partSuccess == chunks.Count())
                successful++;
        }
        if (successful != Files.Count)
            throw new Exception("Not all parts where successfully uploaded.");
    }    
    private async Task GetFileTypeId()
    {
        if(_fileType == null)
        {
            var file = _archiveFiles[0];
            var fileType = await _core.FileTypes.BestGuessAsync(file.Extension) 
                ?? throw new Exception($"A file type couldnot be determined from extension {file.Extension}.");
            fileType = _core.FileTypes.Find(fileType.Id);
            SetFileType(fileType);
        }        
    }
    private async Task AddMetadata()
    {
        _model.KeywordCollection = KeywordCollection.GetModel();
        _model.Uploads = Files.Select(f => new UploadModel { Id = f.Id }).ToList();
        var resp = await Module.Run(Api.PostDocument(_model));
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
