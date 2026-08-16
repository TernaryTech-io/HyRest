namespace HyRest.OnBase.ApiServices;

public partial class OnBaseCoreService : OnBaseService<IOnBaseDocumentAPI>, IOnBaseCoreService
{
    public Task<NoteModel?> GetNote(string id, CancellationToken token = default)
        => Run(Api.GetNoteByNoteId(id), token);
    public Task<NoteModel?> PatchNote(string id, UpdateNoteProperties properties, CancellationToken token = default)
        => Run(Api.PatchNoteByNoteId(id, properties), token);
    public Task DeleteNote(string id, CancellationToken token = default)
        => Run(Api.DeleteNoteByNoteId(id), token);
}