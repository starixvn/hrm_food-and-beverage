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
    List<FnBSyncApiSettings> settings) : IEmployeeService
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
        var company = CompanyIdentity.Sms.GetDescription();
        var setting = settings.FirstOrDefault(x => string.Equals(x.Name, company, StringComparison.CurrentCultureIgnoreCase));
        await SyncEmployees(setting, companyId);
        await SyncTeams(setting, companyId);
        await SyncCalendar(setting, companyId);
    }

    private async Task SyncEmployees(FnBSyncApiSettings setting, int companyId)
    {
        try
        {
            var result = new CommandResult();
            LoggingExtension.LogUpdateNotify(companyId, employeeFunction);
            var lastModifiedDate = await GetEmployeeLastModifiedDate(companyId);
            var employees = await GetEmployeesByLastModifiedDate(setting, lastModifiedDate);
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
        catch (Exception ex)
        {
            LoggingExtension.LogExceptionNotify(companyId, employeeFunction, ex);
        }
    }

    private async Task<List<Employee>?> GetEmployeesByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {

        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        if (!string.IsNullOrWhiteSpace(setting.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.Employee}?lastModifiedDate={lastModifiedDate}");

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

    private async Task SyncTeams(FnBSyncApiSettings setting, int companyId)
    {
        try 
        {
            LoggingExtension.LogUpdateNotify(companyId, teamFunction);
            var result = new CommandResult();
            var lastModifiedDate = await GetEmployeeTeamLastModifiedDate(companyId);
            var teams = await GetTeamByLastModifiedDate(setting, lastModifiedDate);
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
        catch (Exception ex)
        {
            LoggingExtension.LogExceptionNotify(companyId, teamFunction, ex);
        }
    }

    private async Task<List<Team>?> GetTeamByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        if (!string.IsNullOrWhiteSpace(setting.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.EmployeeTeam}?lastModifiedDate={lastModifiedDate}");

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

    private async Task SyncCalendar(FnBSyncApiSettings setting, int companyId)
    {
        try 
        {
            LoggingExtension.LogUpdateNotify(companyId, calendarFunction);
            var result = new CommandResult();
            var lastModifiedDate = await GetEmployeeCalendarLastModifiedDate(companyId);
            var employeeCalendars = await GetCalendarByLastModifiedDate(setting, lastModifiedDate);
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
        catch (Exception ex)
        {
            LoggingExtension.LogExceptionNotify(companyId, calendarFunction, ex);
        }
    }

    private async Task<List<EmployeeCalendar>?> GetCalendarByLastModifiedDate(FnBSyncApiSettings setting, DateTime lastModifiedDate)
    {
        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        if (!string.IsNullOrWhiteSpace(setting.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.EmployeeCalendar}?lastModifiedDate={lastModifiedDate}");

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