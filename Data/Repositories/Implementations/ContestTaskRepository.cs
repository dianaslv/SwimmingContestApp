using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;
using Data.DataContexts.Interfaces;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace Data.Repositories.Implementations
{
    public class ContestTaskRepository : IContestTaskRepository
    {
         private readonly IContestTaskDataContext m_dataContext;
        private readonly ILogger<ContestTaskRepository> m_logger;

        public ContestTaskRepository(IContestTaskDataContext dataContext, ILogger<ContestTaskRepository> logger)
        {
            m_dataContext = dataContext;
            m_logger = logger;
        }

        public async Task CreateAsync(ContestTask entity)
        {
            try
            {
                await m_dataContext.CreateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not create ContestTask");
            }
        }

        public async Task UpdateAsync(ContestTask entity)
        {
            try
            {
                await m_dataContext.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not update ContestTask");
            }
        }

        public async Task DeleteAsync(ContestTask entity)
        {
            try
            {
                await m_dataContext.DeleteAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not delete ContestTask");
            }
        }

        public async Task<List<ContestTask>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            try
            {
                return await m_dataContext.SearchAsync(searchTerms);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not search ContestTask");
            }

            return null;
        }

        public async Task<DbDataAdapter> GetDataAdapterAsync(List<Tuple<string, string>> searchTerms)
        {
            try
            {
                return await m_dataContext.FillDbDataAdapterAsync(searchTerms);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not search ContestTask");
            }

            return null;
        }
    }
}