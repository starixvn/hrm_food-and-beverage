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
    List<FnBSyncApiSettings> settings) : IDepartmentService
{
    private const string function = "Department";
    public async Task StartDepartmentJobAsync()
    {
        await GetSyncSms();
    }

    private async Task GetSyncSms()
    {
        const int companyId = (int)CompanyIdentity.Sms;
        var company = CompanyIdentity.Sms.GetDescription();
        var setting = settings.FirstOrDefault(x => string.Equals(x.Name, company, StringComparison.CurrentCultureIgnoreCase));
        await SyncDepartments(setting, companyId);
    }

    private async Task SyncDepartments(FnBSyncApiSettings setting, int companyId)
    {
        try
        {
            LoggingExtension.LogUpdateNotify(companyId, function);
            var result = new CommandResult();
            var lastModifiedDate = await GetDepartmentLastModifiedDate(companyId);
            var departments = await GetDepartmentsByLastModifiedDate(setting, lastModifiedDate);
            if (departments == null)
            {
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
            LoggingExtension.LogExceptionNotify(companyId, function, ex);
        }
    }

    private async Task<List<Department>?> GetDepartmentsByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient();
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