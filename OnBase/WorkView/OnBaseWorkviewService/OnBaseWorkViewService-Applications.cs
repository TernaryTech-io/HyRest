using HyRest.Cache;
using Microsoft.Extensions.Logging;

namespace HyRest.OnBase.ApiServices;

public partial class OnBaseWorkViewService : OnBaseService<IOnBaseWorkViewAPI>, IOnBaseWorkViewService
{
    public Task<ApplicationCollectionModel?> _getApplications(CancellationToken token = default)
        => Run(Api.Applications(Options.DefaultLanguage), token);
    public async Task<ApplicationCollectionModel?> GetApplications(CancellationToken token = default)
    {
        var col = await _getApplications(token);
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
    public async Task<ApplicationModel?> GetApplication(string id, CancellationToken token = default)
    {
        var col = await _getApplications(token);
        if(col != null)
        {
            var app = col.Items.FirstOrDefault(a => a.Id == id);
            if (app != null)
                await Cache.SetAsync(app, token);
            return app;
        }
        return null;
    }
}