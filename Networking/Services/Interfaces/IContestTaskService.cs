using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;

namespace Networking.Services.Interfaces
{
    public interface IContestTaskService
    {
        Task DeleteContestTask(Guid id);
        Task<List<ContestTask>> SearchContestTasks(List<Tuple<string, string>> searchTerm);
        Task<DbDataAdapter> GetContestTasksDbAdapter(List<Tuple<string, string>> searchTerm);
    }
}