using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Helper;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

public interface IEmployeeService
{
    Task StartEmployeeJobAsync();
}

public class EmployeeService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    FnBSyncApiSettings settings,
    ILogger<Worker> logger) : IEmployeeService
{
    private const string employeeFunction = "Employee";
    private const string teamFunction = "Team";
    private const string calendarFunction = "Calendar";

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

    private async Task SyncEmployees(int companyId)
    {
        LoggingExtension.LogUpdateNotify(companyId, employeeFunction);
        var result = new CommandResult();
        var lastModifiedDate = await GetEmployeeLastModifiedDate(companyId);
        var employees = await GetEmployeesByLastModifiedDate(lastModifiedDate);
        if (employees == null)
        {
            LoggingExtension.LogResultNotify(companyId, result, employeeFunction);
            return;
        }
        var criteria = new SyncEmployeesCriteria
        {
            CompanyId = companyId,
            Users = employees
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        LoggingExtension.LogResultNotify(companyId, result, employeeFunction);
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

    private async Task SyncTeams(int companyId)
    {
        LoggingExtension.LogUpdateNotify(companyId, teamFunction);
        var result = new CommandResult();
        var lastModifiedDate = await GetEmployeeTeamLastModifiedDate(companyId);
        var teams = await GetTeamByLastModifiedDate(lastModifiedDate);
        if (teams == null)
        {
            LoggingExtension.LogResultNotify(companyId, result, teamFunction);
            return;
        }
        var criteria = new SyncEmployeeTeamCriteria
        {
            CompanyId = companyId,
            Teams = teams
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        LoggingExtension.LogResultNotify(companyId, result, teamFunction);
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

    private async Task SyncCalendar(int companyId)
    {
        LoggingExtension.LogUpdateNotify(companyId, calendarFunction);
        var result = new CommandResult();
        var lastModifiedDate = await GetEmployeeCalendarLastModifiedDate(companyId);
        var employeeCalendars = await GetCalendarByLastModifiedDate(lastModifiedDate);
        if (employeeCalendars == null)
        {
            LoggingExtension.LogResultNotify(companyId, result, calendarFunction);
            return;
        }
        var criteria = new SyncEmployeesCalendarCriteria
        {
            CompanyId = companyId,
            EmployeeCalendars = employeeCalendars
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        LoggingExtension.LogResultNotify(companyId, result, calendarFunction);
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