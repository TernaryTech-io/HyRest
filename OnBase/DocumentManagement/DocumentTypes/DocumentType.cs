using System.Text.Json.Serialization;
using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public sealed class DocumentType : OnBaseItemTypeService<OnBaseCore, DocumentTypeModel>
{
    private FileType? _fileType { get; set; }
    private AutoFillKeywordSet? _autoFillKeywordSet { get; set; }
    private DocumentTypeGroup? _documentTypeGroup{ get; set; }
    private KeywordTypeCollection? _keywordTypeCollection { get; set; }
    public DocumentType(OnBaseCore core, DocumentTypeModel docType) : base(core, docType)
    {

    }
    public FileType? DefaultFileType
    {
        get
        {
            if (_fileType == null)
                PopulateDefaultFileType();
            return _fileType;
        }
    }
    [JsonIgnore]
    public string? DocumentDateDisplayName => Item.DocumentDateDisplayName;
    public AutoFillKeywordSet? AutoFillKeywordSet
    {
        get
        {
            if (_autoFillKeywordSet == null)
                GetAutoFillKeywordSet();
            return _autoFillKeywordSet;
        }
    }
    public DocumentTypeGroup? DocumentTypeGroup
    {
        get
        {
            if (_documentTypeGroup == null)
                GetDocumentTypeGroup();
            return _documentTypeGroup;                
        }
    }
    [JsonIgnore] //To Slow for retreiving all document types.
    public KeywordTypeCollection KeywordTypeCollection
    {
        get
        {
            if (_keywordTypeCollection == null)
                PopulateKeywordTypes().Wait(Module.App.RequestTimeOut);
            return _keywordTypeCollection;
        }
    }    
    public KeywordCollection GetDefaultKeywords()
    {
        var task = GetDefaultKeywordsAsync();
        if (task.Wait(Module.App.RequestTimeOut) && task.IsCompletedSuccessfully)
            return task.Result;
        else
            throw task.Exception?.InnerException ?? task.Exception ?? new Exception("Failed to retrieve default keywords");
    }
    public async Task<KeywordCollection> GetDefaultKeywordsAsync(CancellationToken token = default)
    {
        var model = await Module.Service.GetDefaultKeywordsForDocumentType(Item.Id, token);
        if (model != null)
            return new KeywordCollection(Module, model);
        throw new Exception("Could not retrieve the default keywords for this document type.");
    }

    public DocumentArchiveProperties CreateNewDocumentArchiveProperties()
    {
        return new DocumentArchiveProperties(Module, this);
    }
    private async Task PopulateKeywordTypes()
    {
        var ktgcol = await Module.Service.GetKeywordTypeGroupsForDocumentType(Item.Id);
        if (ktgcol != null)
        {
            _keywordTypeCollection = new KeywordTypeCollection(Module, ktgcol);
        }
    }
    private void PopulateDefaultFileType()
    {
        if(Item.DefaultFileTypeId != null)
        {
            _fileType = Module.FileTypes.Find(Item.DefaultFileTypeId); 
        }
    }
    private void GetAutoFillKeywordSet()
    {
        if(Item.AutofillKeywordSetId != null)
        {
            var afks = Module.AutoFillKeywordSets.Find(Item.AutofillKeywordSetId);
            if (afks != null && afks is AutoFillKeywordSet a)
                _autoFillKeywordSet = a;
        }
    }
    private void GetDocumentTypeGroup()
    {
        if (Item.DocumentTypeGroupId != null)
        {
            var dtg = Module.DocumentTypeGroups.Find(Item.DocumentTypeGroupId);
            if (dtg != null && dtg is DocumentTypeGroup docTypeGrp)
                _documentTypeGroup = docTypeGrp;
        }
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
