namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    private Task<AttributeCollectionModel?> _getAttributes(string classId, CancellationToken token = default)
        => Run(Api.Attributes(classId, Options.DefaultLanguage), token);

    public async Task<AttributeCollectionModel?> GetAttributes(string classId, CancellationToken token = default)
    {
        var col = await _getAttributes(classId, token);
        if (col != null)
        {
            col.Items.ToList().ForEach(async i =>
            {
                await Cache.SetAsync(i, token);
            });
            return col;
        }
        else
            return null;
    }
    public async Task<AttributeModel?> GetAttribute(string id, string classId, CancellationToken token = default)
    {
        var col = await _getAttributes(classId, token);
        if (col != null)
        {
            var model = col.Items.FirstOrDefault(a => a.Id == id);
            if(model != null)
                await Cache.SetAsync(model, token);
            return model;
        }
        return null;
    }
}