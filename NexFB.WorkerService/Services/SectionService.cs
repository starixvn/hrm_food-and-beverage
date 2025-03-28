using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Helper;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

public interface ISectionService
{
    Task StartSectionJobAsync();
}
public class SectionService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    List<FnBSyncApiSettings> settings) : ISectionService
{
   private const string function = "Section";
    public async Task StartSectionJobAsync()
    {
        await GetSyncSms();
    }

    private async Task GetSyncSms()
    {
        const int companyId = (int)CompanyIdentity.Sms;
        var company = CompanyIdentity.Sms.GetDescription();
        var setting = settings.FirstOrDefault(x => string.Equals(x.Name, company, StringComparison.CurrentCultureIgnoreCase));
        await SyncSections(setting, companyId);
    }

    private async Task SyncSections(FnBSyncApiSettings setting, int companyId)
    {
        try
        {
            LoggingExtension.LogUpdateNotify(companyId, function);
            var result = new CommandResult();
            var lastModifiedDate = await GetSectionLastModifiedDate(companyId);
            var sections = await GetSectionsByLastModifiedDate(setting, lastModifiedDate);
            if (sections == null)
            {
                LoggingExtension.LogResultNotify(companyId, result, function);
                return;
            }
            var criteria = new SyncSectionCriteria
            {
                CompanyId = companyId,
                Sections = sections
            };

            result = await baseService.ExecuteNonQueryAsync(criteria);
            LoggingExtension.LogResultNotify(companyId, result, function);
        }
        catch (Exception ex)
        {
            LoggingExtension.LogExceptionNotify(companyId, function, ex);
        }
    }

    private async Task<List<Section>?> GetSectionsByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(setting.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.Section}?lastModifiedDate={lastModifiedDate}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<Section>>(content);
        return result ?? null;
    }

    private async Task<DateTime> GetSectionLastModifiedDate(int companyId)
    {
        var criteria = new GetSectionLastModifiedDateCriteria
        {
            CompanyId = companyId
        };
        return await baseService.GetDataEntityAsync<DateTime>(criteria);
    }
}