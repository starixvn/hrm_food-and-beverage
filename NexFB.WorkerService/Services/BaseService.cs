using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Data.Repository;

public interface IBaseService
{
    Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria);

    Task<T> GetDataEntityAsync<T>(ICriteria criteria);

    Task<CommandResult> ExecuteNonQueryAsync(ICriteria command);
}

public class BaseService(IDynamicRepository dynamicRepository,
    IBaseRepository baseRepository) : IBaseService
{
    public async Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria)
    {
        if (criteria.IsValid())
        {
            return await dynamicRepository.FetchAsync<T>(criteria);
        }

        return new List<T>();
    }

    public async Task<T> GetDataEntityAsync<T>(ICriteria criteria)
    {
        if (criteria.IsValid())
        {
            return await dynamicRepository.GetAsync<T>(criteria);
        }

        return default!;
    }

    public async Task<CommandResult> ExecuteNonQueryAsync(ICriteria command)
    {
        var commandResult = new CommandResult();
        if (command.IsValid())
        {
            commandResult = await baseRepository.ExecuteNonQueryAsync(command);
        }

        return commandResult;
    }
}