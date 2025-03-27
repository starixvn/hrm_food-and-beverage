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

    public interface IDepartmentService
    {
        Task StartDepartmentJobAsync();
    }
    public class DepartmentService(IBaseService baseService,
        IHttpClientFactory httpClientFactory,
        FnBSyncApiSettings settings) : IDepartmentService
    {
        public async Task StartDepartmentJobAsync()
        {

            await GetSyncSms();
        }

        private async Task GetSyncSms()
        {
            const int companyId = (int)CompanyIdentity.Sms;
            await SyncDepartments(companyId);
        }

        private async Task<CommandResult> SyncDepartments(int companyId)
        {
            var result = new CommandResult();
            var departments = await GetDepartmentsByLastModifiedDate(companyId);
            if (departments == null) return result;
            var criteria = new SyncDepartmentCriteria()
            {
                CompanyId = companyId,
                Departments = departments
            };
            result = await baseService.ExecuteNonQueryAsync(criteria);
            return result;
        }

        private async Task<List<Department>?> GetDepartmentsByLastModifiedDate(int companyId)
        {

            var lastModifiedDate = await GetDepartmentLastModifiedDate(companyId);
            var httpClient = httpClientFactory.CreateClient();
            if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

            var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.Department}?lastModifiedDate={lastModifiedDate}");

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
}
