using HyRest.Utilities;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HyRest.DocumentManagement;

public sealed class DocumentReindexProperties : OnBaseRestService
{
    private OnBaseCore _core => (OnBaseCore)Module;
    private readonly Document _doc;
    private readonly DocumentReindexPropertiesModel _model;
    private DocumentType _docType { get; set; }
    private FileType _fileType { get; set; }
    internal DocumentReindexProperties(OnBaseCore core, Document doc) : base(core)
    {
        _doc = doc;
        _model = new DocumentReindexPropertiesModel()
        {
            KeywordCollection = _doc.KeywordCollection.GetModel()
        }; 
    }
    /// <summary>
    /// The document type to be reindexed into.
    /// </summary>
    public DocumentType TargetDocumentType { get => _docType; set => SetDocumentTypeAsync(value).Wait(Module.App.ClientOptions.RequestTimeOut); }
    private async Task SetDocumentTypeAsync(DocumentType value)
    {
        _docType = value;
        _model.TargetDocumentTypeId = value.Id.ToString();
        var keywordGuid = _model.KeywordCollection.KeywordGuid;
        var newColl = await value.GetDefaultKeywordsAsync();
        var newModel = newColl.GetModel();
        newModel.KeywordGuid = keywordGuid;
        _model.KeywordCollection = newModel;
    }

    /// <summary>
    /// The file type to be reindexed into. This is only necessary if attempting to change the
    /// <br/>file type ID of the default rendition of the latest revision.
    /// </summary>
    public FileType? TargetFileType { get => _fileType; set => SetFileType(value); }
    private void SetFileType(FileType value)
    {
        _fileType = value;
        _model.TargetFileTypeId = value.ToString();
    }

    /// <summary>
    /// Boolean indicating if the document should be reindexed as specified.
    /// <br/>This should be used in conjunction with a Revisable/Renditionable document type to
    /// <br/>indicate that the document should be reindexed as specified regardless of the document type
    /// <br/>settings for revisions and renditions.
    /// <br/>This would be considered false by default and if it's a Revisable/Renditionable document type,
    /// <br/>existing documents are checked to find matching documents for which this new document can be
    /// <br/>added as a Revision/Rendition.
    /// </summary>
    public bool StoreAsNew { get => _model.StoreAsNew; set => _model.StoreAsNew = value; }

    /// <summary>
    /// The revision comment that will be saved during reindex if the document is
    /// <br/>revisiable.
    /// </summary>
    public string? Comment { get => _model.Comment; set => _model.Comment = value; }

    /// <summary>
    /// The document date.
    /// </summary>
    public DateTime DocumentDate { get => _model.DocumentDate.Date; set => _model.DocumentDate = value; }

    /// <summary>
    /// An array of keywords grouped by the keyword group they belong to.
    /// </summary>
    public KeywordCollection KeywordCollection { get => new KeywordCollection(_core, _model.KeywordCollection); set => _model.KeywordCollection = value.GetModel(); }

    internal DocumentReindexPropertiesModel GetModel() => _model;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
