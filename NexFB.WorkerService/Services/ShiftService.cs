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

public interface IShiftService
{
    Task StartShiftJobAsync();
}

public class ShiftService(IBaseService baseService,
    IHttpClientFactory httpClientFactory,
    List<FnBSyncApiSettings> settings,
    Fanex.Logging.ILogger logger) : IShiftService
{
    private const string function = "Shift";

    public async Task StartShiftJobAsync()
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
        await SyncShifts(setting, companyId);
    }

    private async Task SyncShifts(FnBSyncApiSettings setting, int companyId)
    {
        try
        {
            LoggingExtension.LogUpdateNotify(companyId, function);
            var result = new CommandResult();
            var shifts = await GetShifts(setting);
            if (shifts == null || !shifts.Any())
            {
                result.Message = Messages.NoRecordToBeSync;
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
        catch (Exception ex)
        {
            await logger.ErrorAsync($"Error sync shift {companyId}, Message: {ex.Message}", ex);
        }
    }

    private async Task<List<Shift>> GetShifts(FnBSyncApiSettings setting)
    {
        var httpClient = httpClientFactory.CreateClient(Constants.FnBClient);
        if (!string.IsNullOrWhiteSpace(setting.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-Key", setting.ApiKey);
        }

        var response = await httpClient.GetAsync($"{setting.Url}/{Endpoint.Shift}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(content) ? null : JsonConvert.DeserializeObject<List<Shift>>(content);
    }
}