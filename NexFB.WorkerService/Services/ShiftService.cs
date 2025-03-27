using Newtonsoft.Json;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Criteria;
using NexFB.WorkerService.Helper;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

public interface IShiftService
{
    Task StartShiftJobAsync();
}
public class ShiftService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    FnBSyncApiSettings settings) : IShiftService
{
    private const string function = "Shift";

    public async Task StartShiftJobAsync()
    {
        await GetSyncSms();
    }

    private async Task GetSyncSms()
    {
        const int companyId = (int)CompanyIdentity.Sms;
        await SyncShifts(companyId);
    }

    private async Task SyncShifts(int companyId)
    {
        LoggingExtension.LogUpdateNotify(companyId, function);
        var result = new CommandResult();
        var shifts = await GetShifts();
        if (shifts == null)
        {
            LoggingExtension.LogResultNotify(companyId, result, function);
            return;
        }
        var criteria = new SyncShiftCriteria
        {
            CompanyId = companyId,
            Shifts = shifts
        };

        result = await baseService.ExecuteNonQueryAsync(criteria);
        LoggingExtension.LogResultNotify(companyId, result, function);
    }

    private async Task<List<Shift>?> GetShifts()
    {
        var httpClient = httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(settings.ApiKey)) httpClient.DefaultRequestHeaders.Add("X-API-Key", settings.ApiKey);

        var response = await httpClient.GetAsync($"{settings.Url}/{Endpoint.Shift}");

        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return null;
        var result = JsonConvert.DeserializeObject<List<Shift>>(content);
        return result ?? null;
    }
}