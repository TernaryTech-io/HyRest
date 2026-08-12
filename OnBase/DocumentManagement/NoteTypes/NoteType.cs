using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class NoteType : OnBaseItemTypeService<OnBaseCore, NoteTypeModel>
{
    public NoteType(OnBaseCore core, NoteTypeModel item) : base(core,item)
    {

    }
    public NoteColor Color => Item.Color;
    public NoteTypeDisplayFlags DisplayFlags => Item.DisplayFlags;
    public NoteTypeModelFlavor Flavor => Item.Flavor;
    public string? FontId => Item.FontId;
    public string? IconId => Item.IconId;
    public NoteTypeUserPrivileges UserPrivileges => Item.UserPrivileges;

    public AddNoteProperties CreateAddNoteProperties()
    {
        return new AddNoteProperties
        {
            NoteTypeId = Item.Id,
        };
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
