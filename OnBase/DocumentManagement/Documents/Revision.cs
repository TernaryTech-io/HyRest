

using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class Revision : OnBaseItemService<IOnBaseDocumentAPI, OnBaseCore, RevisionModel>
{
    private readonly DocumentModel _doc;
    private List<Rendition> _renditions { get; set; } = [];
    public Revision(OnBaseCore core, DocumentModel doc, RevisionModel revision) : base(core, revision)
    {
        _doc = doc;
    }
    public int RevisionNumber => Item.RevisionNumber;
    public List<Rendition> Renditions
    {
        get
        {
            if (_renditions == null)
                PopulateRenditions().Wait();
            return _renditions;
        }
    }
    private async Task PopulateRenditions()
    {
        var renCol = await Module.Run(Api.GetRenditionCollectionForRevisionOfDocument(_doc.Id, Item.Id));
        if (renCol != null && renCol.Items.Count > 0)
            _renditions = renCol.Items.Select(i => new Rendition(Module, Item, i)).ToList();
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
