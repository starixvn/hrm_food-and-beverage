using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Helper;
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
    private const string function = "Unit";

    public async Task StartUnitJobAsync()
    {
        await GetSyncSms();
    }

    private async Task GetSyncSms()
    {
        const int companyId = (int)CompanyIdentity.Sms;
        await SyncUnits(companyId);
    }

    private async Task SyncUnits(int companyId)
    {
        LoggingExtension.LogUpdateNotify(companyId, function);
        var result = new CommandResult();
        var lastModifiedDate = await GetUnitLastModifiedDate(companyId);
        var units = await GetUnitsByLastModifiedDate(lastModifiedDate);
        if (units == null)
        {
            LoggingExtension.LogResultNotify(companyId, result, function);
            return;
        }
        var criteria = new SyncUnitCriteria
        {
            CompanyId = companyId,
            Units = units
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        LoggingExtension.LogResultNotify(companyId, result, function);
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