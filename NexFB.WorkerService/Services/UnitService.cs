using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

public interface IUnitService
{
    Task StartUnitJobAsync();
}
public class UnitService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    FnBSyncApiSettings settings) : IUnitService
{
    public async Task StartUnitJobAsync()
    {
        await GetSyncSms();
    }

    private async Task GetSyncSms()
    {
        const int companyId = (int)CompanyIdentity.Sms;
        await SyncUnits(companyId);
    }

    private async Task<CommandResult> SyncUnits(int companyId)
    {
        var result = new CommandResult();
        var lastModifiedDate = await GetUnitLastModifiedDate(companyId);
        var units = await GetUnitsByLastModifiedDate(lastModifiedDate);
        if (units == null) return result;
        var criteria = new SyncUnitCriteria
        {
            CompanyId = companyId,
            Units = units
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<List<Unit>?> GetUnitsByLastModifiedDate(DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

        var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.Unit}?lastModifiedDate={lastModifiedDate}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<Unit>>(content);
        return result ?? null;
    }

    private async Task<DateTime> GetUnitLastModifiedDate(int companyId)
    {
        var criteria = new GetUnitLastModifiedDateCriteria
        {
            CompanyId = companyId
        };
        return await baseService.GetDataEntityAsync<DateTime>(criteria);
    }
}