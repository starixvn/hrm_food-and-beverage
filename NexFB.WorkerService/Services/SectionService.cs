using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Models;
using static Dapper.SqlMapper;

namespace NexFB.WorkerService.Services
{
    public interface ISectionService
    {
        Task StartSectionJobAsync();
    }
    public class SectionService(IBaseService baseService,
        IHttpClientFactory httpClientFactory,
        FnBSyncApiSettings settings) : ISectionService
    {
        public async Task StartSectionJobAsync()
        {
            await GetSyncSms();
        }

        private async Task GetSyncSms()
        {
            const int companyId = (int)CompanyIdentity.Sms;
            await SyncSections(companyId);
        }

        private async Task<CommandResult> SyncSections(int companyId)
        {
            var result = new CommandResult();
            var lastModifiedDate = await GetSectionLastModifiedDate(companyId);
            var sections = await GetSectionsByLastModifiedDate(lastModifiedDate);
            if (sections == null) return result;
            var criteria = new SyncSectionCriteria
            {
                CompanyId = companyId,
                Sections = sections
            };

            result = await baseService.ExecuteNonQueryAsync(criteria);
            return result;
        }

        private async Task<List<Section>?> GetSectionsByLastModifiedDate(DateTime lastModifiedDate)
        {
            var httpClient = httpClientFactory.CreateClient();
            if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

            var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.Section}?lastModifiedDate={lastModifiedDate}");

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
}
