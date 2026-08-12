using HyRest.Utilities;
using HyRest.FileTypeMapping;

namespace HyRest.DocumentManagement;

public sealed class FileType : OnBaseItemTypeService<OnBaseCore, FileTypeModel>
{   
    public FileType(OnBaseCore core, FileTypeModel fileType) : base(core, fileType)
    {
        
    }
    public string? Extension => FileTypeMap.GetExtension(Id);
    public string? MimeType => FileTypeMap.GetMimeType(Id);
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
