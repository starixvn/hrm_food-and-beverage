using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
    List<FnBSyncApiSettings> settings,
    Fanex.Logging.ILogger logger) : ISectionService
{
   private const string function = "Section";
    public async Task StartSectionJobAsync()
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
            if (sections == null || !sections.Any())
            {
                result.Message = Messages.NoRecordToBeSync;
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
            await logger.ErrorAsync($"Error sync section {companyId}, Message: {ex.Message}", ex);
        }
    }

    private async Task<List<Section>> GetSectionsByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        if (!string.IsNullOrWhiteSpace(setting.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);
        }

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.Section}?lastModifiedDate={lastModifiedDate}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(content) ? null : JsonConvert.DeserializeObject<List<Section>>(content);
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