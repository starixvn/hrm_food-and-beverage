using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

public interface IEmployeeService
{
    Task StartEmployeeJobAsync();
}

public class EmployeeService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    FnBSyncApiSettings settings) : IEmployeeService
{
    public async Task StartEmployeeJobAsync()
    {
        await GetSyncSms();
    }

    private async Task GetSyncSms()
    {
        const int companyId = (int)CompanyIdentity.Sms;
        await SyncEmployees(companyId);
        await SyncTeams(companyId);
        await SyncCalendar(companyId);
    }

    private async Task<CommandResult> SyncEmployees(int companyId)
    {
        var result = new CommandResult();
        var employees = await GetEmployeesByLastModifiedDate(companyId);
        if (employees == null) return result;
        var criteria = new SyncEmployeesCriteria
        {
            CompanyId = companyId,
            Users = employees
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<CommandResult> SyncTeams(int companyId)
    {
        var result = new CommandResult();
        var teams = await GetTeamByLastModifiedDate(companyId);
        if (teams == null) return result;
        var criteria = new SyncEmployeeTeamCriteria
        {
            CompanyId = companyId,
            Teams = teams
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<CommandResult> SyncCalendar(int companyId)
    {
        var result = new CommandResult();
        var employeeCalendars = await GetCalendarByLastModifiedDate(companyId);
        if (employeeCalendars == null) return result;
        var criteria = new SyncEmployeesCalendarCriteria
        {
            CompanyId = companyId,
            EmployeeCalendars = employeeCalendars
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<List<EmployeeCalendar>?> GetCalendarByLastModifiedDate(int companyId)
    {
        var lastModifiedDate = await GetEmployeeCalendarLastModifiedDate(companyId);

        var httpClient = httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

        var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.EmployeeCalendar}?lastModifiedDate={lastModifiedDate}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<EmployeeCalendar>>(content);
        return result ?? null;
    }

    private async Task<DateTime> GetEmployeeCalendarLastModifiedDate(int companyId)
    {
        var criteria = new GetEmployeeTeamLastModifiedDateCriteria
        {
            CompanyId = companyId
        };
        return await baseService.GetDataEntityAsync<DateTime>(criteria);
    }

    private async Task<List<Team>?> GetTeamByLastModifiedDate(int companyId)
    {
        var lastModifiedDate = await GetEmployeeTeamLastModifiedDate(companyId);

        var httpClient = httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

        var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.EmployeeTeam}?lastModifiedDate={lastModifiedDate}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<Team>>(content);
        return result ?? null;
    }

    private async Task<DateTime> GetEmployeeTeamLastModifiedDate(int companyId)
    {
        var criteria = new GetEmployeeTeamLastModifiedDateCriteria
        {
            CompanyId = companyId
        };
        return await baseService.GetDataEntityAsync<DateTime>(criteria);
    }

    private async Task<List<Employee>?> GetEmployeesByLastModifiedDate(int companyId)
    {
        var lastModifiedDate = await GetEmployeeLastModifiedDate(companyId);

        var httpClient = httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

        var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.Employee}?lastModifiedDate={lastModifiedDate}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<Employee>>(content);
        return result ?? null;
    }

    private async Task<DateTime> GetEmployeeLastModifiedDate(int companyId)
    {
        var criteria = new GetEmployeeLastModifiedDateCriteria
        {
            CompanyId = companyId
        };
        return await baseService.GetDataEntityAsync<DateTime>(criteria);
    }
}