using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Helper;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

public interface IDepartmentService
{
    Task StartDepartmentJobAsync();
}
public class DepartmentService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    List<FnBSyncApiSettings> settings,
    Fanex.Logging.ILogger logger) : IDepartmentService
{
    private const string function = "Department";
    public async Task StartDepartmentJobAsync()
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
        await SyncDepartments(setting, companyId);
    }

    private async Task SyncDepartments(FnBSyncApiSettings setting, int companyId)
    {
        try
        {
            var result = new CommandResult();
            var lastModifiedDate = await GetDepartmentLastModifiedDate(companyId);
            var departments = await GetDepartmentsByLastModifiedDate(setting, lastModifiedDate);
            if (departments == null || !departments.Any())
            {
                result.Message = Messages.NoRecordToBeSync;
                LoggingExtension.LogResultNotify(companyId, result, function);
                return;
            }
            var criteria = new SyncDepartmentCriteria
            {
                CompanyId = companyId,
                Departments = departments
            };
            result = await baseService.ExecuteNonQueryAsync(criteria);
            LoggingExtension.LogResultNotify(companyId, result, function);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"Error sync department {companyId}, Message: {ex.Message}", ex);
        }
    }

    private async Task<List<Department>?> GetDepartmentsByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        var handler = new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
        httpClient = new HttpClient(handler);
        
        if (!string.IsNullOrWhiteSpace(setting.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.Department}?lastModifiedDate={lastModifiedDate}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<Department>>(content);
        return result ?? null;
    }

    private async Task<DateTime> GetDepartmentLastModifiedDate(int companyId)
    {
        var criteria = new GetDepartmentLastModifiedDateCriteria
        {
            CompanyId = companyId
        };
        return await baseService.GetDataEntityAsync<DateTime>(criteria);
    }
}