using System.Text.Json.Serialization;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public sealed class DocumentType : OnBaseItemTypeService<IOnBaseDocumentAPI, OnBaseCore, DocumentTypeModel>
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
                PopulateKeywordTypes().Wait();
            return _keywordTypeCollection;
        }
    }    
    public async Task<KeywordCollection> GetDefaultKeywords()
    {
        var model = await Module.Run(Api.GetDefaultKeywordCollectionForDocumentType(Item.Id, Options.DefaultLanguage));
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
        var ktgcol = await Module.Run(Api.GetKeywordTypeGroupCollectionForDocumentType(Item.Id, Options.DefaultLanguage));
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
