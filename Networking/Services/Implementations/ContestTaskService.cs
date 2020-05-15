using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;
using Data.Repositories.Interfaces;
using Networking.Services.Interfaces;

namespace Networking.Services.Implementations
{
    public class ContestTaskService : IContestTaskService
    {
        private readonly IContestTaskRepository repository;

        public ContestTaskService(IContestTaskRepository repository)
        {
            this.repository = repository;
        }

        public async Task DeleteContestTask(Guid id)
        {
            await repository.DeleteAsync(new ContestTask {Id = id});
        }

        public async Task<List<ContestTask>> SearchContestTasks(List<Tuple<string, string>> searchTerm)
        {
            return await repository.SearchAsync(searchTerm);
        }

        public async Task<DbDataAdapter> GetContestTasksDbAdapter(List<Tuple<string, string>> searchTerm)
        {
            return await repository.GetDataAdapterAsync(searchTerm);
        }
    }
}