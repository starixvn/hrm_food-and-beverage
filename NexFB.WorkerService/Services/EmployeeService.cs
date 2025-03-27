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
        var lastModifiedDate = await GetEmployeeLastModifiedDate(companyId);
        var employees = await GetEmployeesByLastModifiedDate(lastModifiedDate);
        if (employees == null) return result;
        var criteria = new SyncEmployeesCriteria
        {
            CompanyId = companyId,
            Users = employees
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<List<Employee>?> GetEmployeesByLastModifiedDate(DateTime lastModifiedDate)
    {
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

    private async Task<CommandResult> SyncTeams(int companyId)
    {
        var result = new CommandResult();
        var lastModifiedDate = await GetEmployeeTeamLastModifiedDate(companyId);
        var teams = await GetTeamByLastModifiedDate(lastModifiedDate);
        if (teams == null) return result;
        var criteria = new SyncEmployeeTeamCriteria
        {
            CompanyId = companyId,
            Teams = teams
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<List<Team>?> GetTeamByLastModifiedDate(DateTime lastModifiedDate)
    {
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

    private async Task<CommandResult> SyncCalendar(int companyId)
    {
        var result = new CommandResult();
        var lastModifiedDate = await GetEmployeeCalendarLastModifiedDate(companyId);
        var employeeCalendars = await GetCalendarByLastModifiedDate(lastModifiedDate);
        if (employeeCalendars == null) return result;
        var criteria = new SyncEmployeesCalendarCriteria
        {
            CompanyId = companyId,
            EmployeeCalendars = employeeCalendars
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        return result;
    }

    private async Task<List<EmployeeCalendar>?> GetCalendarByLastModifiedDate(DateTime lastModifiedDate)
    {
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
}