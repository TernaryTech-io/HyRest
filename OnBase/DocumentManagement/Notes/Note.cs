using Ternary.DataConversions.Extensions;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;
public class Note : OnBaseItemService<IOnBaseDocumentAPI, OnBaseCore, NoteModel>
{
    private NoteType? _noteType { get; set; }
    internal Note(OnBaseCore core, NoteModel item) : base(core,item)
    {
        
    }
    public NoteType? NoteType
    {
        get
        {
            if (_noteType == null)
                GetNoteType();
            return _noteType;
        }
    }
    public string Text => Item.Text ?? string.Empty;
    public string CreatedUserId => Item.CreatedUserId ?? string.Empty;
    public DateTime Created => Item.Created.ConvertTo<DateTime>();
    public long DocumentId => Item.DocumentId.ConvertTo<long>();
    public long DocumentRevisionId => Item.DocumentRevisionId.ConvertTo<long>();
    public long Page => Item.Page;
    public int X => Item.X;
    public int Y => Item.Y;
    public int Width => Item.Width;
    public int Height => Item.Height;
    public NotePrivileges Privileges => Item.Privileges;
    public NoteDisplayFlags DisplayFlags => Item.DisplayFlags;
    public UpdateNoteProperties CreateUpdateNoteProperties()
    {
        return new UpdateNoteProperties
        {
            Text = this.Text,
            Position = new UpdateNotePosition
            {
                X = this.X,
                Y = this.Y
            },
            Size = new UpdateNoteSize
            {
                Height = this.Height,
                Width = this.Width
            }
        };
    }

    public async Task<Note> UpdateAsync(UpdateNoteProperties properties)
    {
        var model = await Module.Run(Api.PatchNoteByNoteId(Item.Id, properties));
        if (model != null)
            return new Note(Module, model);
        return this;
    }
    public Task DeleteAsync()
        => Module.Run(Api.DeleteNoteByNoteId(Item.Id));   
    private void GetNoteType()
    {
        var item = Module.NoteTypes.Find(Item.NoteTypeId);
        if (item != null && item is NoteType nt)
            _noteType = nt;
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
