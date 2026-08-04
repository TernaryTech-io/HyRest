using HyRest.Utilities;
using System.Text.Json.Serialization;

namespace HyRest.DocumentManagement;

public sealed class DocumentTypeGroup : OnBaseItemTypeService<IOnBaseDocumentAPI, OnBaseCore, DocumentTypeGroupModel>
{
    private List<DocumentType> _documentTypes {  get; set; }
    public DocumentTypeGroup(OnBaseCore core, DocumentTypeGroupModel item) : base(core, item)
    {
    }
    [JsonIgnore] 
    public IReadOnlyCollection<DocumentType> DocumentTypes
    {
        get
        {
            if(_documentTypes == null)
                PopulateDocumentTypes().Wait();
            return _documentTypes;
        }
    }
    private async Task PopulateDocumentTypes()
    {
        var col = await Module.Run(Api.GetDocumentTypeCollectionForDocumentTypeGroup(Item.Id));
        if(col != null & col.Items.Count > 0)
        {
            List<DocumentType> list = [];
            col.Items.ToList().ForEach(d => 
            {
                var dt = Module.DocumentTypes[d.Id];
                if (dt != null)
                    list.Add(dt);
            });
            _documentTypes = list;
        }        
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}
