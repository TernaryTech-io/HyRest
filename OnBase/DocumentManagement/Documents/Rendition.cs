using Ternary.DataConversions.Extensions;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class Rendition : OnBaseItemService<OnBaseCore, RenditionModel>
{
    private readonly RevisionModel _revision;
    private FileType? _fileType { get; set; }
    public Rendition(OnBaseCore core, RevisionModel revision, RenditionModel rendition) : base(core, rendition)
    {
        _revision = revision;
    }
    public FileType? FileType
    {
        get
        {
            if (_fileType == null)
                GetFileType();
            return _fileType;
        }
    }
    public DateTime Created => Item.Created.ConvertTo<DateTime>();
    public int PageCount => Item.PageCount;
    public string CreatedByUserId => Item.CreatedByUserId ?? string.Empty;
    public string Comment => Item.Comment ?? string.Empty;
    public void GetFileType()
    {
        var item = Module.FileTypes.Find(Item.Id);
        if (item != null && item is FileType ft)
            _fileType = ft;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
