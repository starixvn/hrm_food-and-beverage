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
    List<FnBSyncApiSettings> settings,
    Fanex.Logging.ILogger logger) : IUnitService
{
    private const string function = "Unit";

    public async Task StartUnitJobAsync()
    {
        foreach (var setting in settings)
        {
            var companyName = setting.Name;
            var companyId = (int)Enum.Parse<CompanyIdentity>(companyName, true);
            if (companyId == (int)CompanyIdentity.None)
            {
                throw new ArgumentException($"Company {companyName} not found");
            }

            await GetSync(setting, companyId);
        }
    }

    private async Task GetSync(FnBSyncApiSettings setting, int companyId)
    {
        await SyncUnits(setting, companyId);
    }

    private async Task SyncUnits(FnBSyncApiSettings setting, int companyId)
    {
        try
        {
            LoggingExtension.LogUpdateNotify(companyId, function);
            var result = new CommandResult();
            var lastModifiedDate = await GetUnitLastModifiedDate(companyId);
            var units = await GetUnitsByLastModifiedDate(setting, lastModifiedDate);
            if (units == null || !units.Any())
            {
                result.Message = Messages.NoRecordToBeSync;
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
        catch (Exception ex)
        {
            logger.Error(companyId.ToString(), function, ex);
        }
    }

    private async Task<List<Unit>?> GetUnitsByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
        httpClient = new HttpClient(handler);
        
        if (!string.IsNullOrWhiteSpace(setting.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.Unit}?lastModifiedDate={lastModifiedDate}");

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