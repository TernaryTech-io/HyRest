using Ternary.DataConversions.Extensions;
using HyRest.Utilities;
using System.Text.Json.Serialization;

namespace HyRest.OnBase.Core;
public class Note : OnBaseItemService<OnBaseCore, NoteModel>
{
    private NoteType? _noteType { get; set; }
    private User? _createdByUser { get; set; }
    internal Note(OnBaseCore core, NoteModel item) : base(core,item)
    {
        
    }
    public override string? TypeId => Item.NoteTypeId;
    [JsonIgnore]
    public NoteType NoteType
    {
        get
        {
            if (_noteType == null)
                GetNoteType();
            return _noteType;
        }
    }
    public string Text => Item.Text ?? string.Empty;
    public string CreatedByUserId => Item.CreatedUserId ?? string.Empty;
    [JsonIgnore]
    public User CreatedByUser
    {
        get
        {
            if (_createdByUser == null)
                GetCreatedByUser();
            return _createdByUser;
        }
    }
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

    public async Task<Note> UpdateAsync(UpdateNoteProperties properties, CancellationToken token = default)
    {
        var model = await Module.Service.PatchNote(Item.Id, properties, token);
        if (model != null)
            return new Note(Module, model);
        return this;
    }
    public Task DeleteAsync(CancellationToken token = default)
        => Module.Service.DeleteNote(Item.Id, token);   
    private void GetNoteType()
    {
        var item = Module.NoteTypes.Find(Item.NoteTypeId);
        if (item != null && item is NoteType nt)
            _noteType = nt;
    }
    private void GetCreatedByUser()
    {
        if(CreatedByUserId != null)
        {
            var admin = (OnBaseAdministration)Module.App.Administration;
            _createdByUser = admin.Users[CreatedByUserId];
        }
        
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
