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
    public class EntryRepository : IEntryRepository
    {
         private readonly IEntryDataContext m_dataContext;
        private readonly ILogger<EntryRepository> m_logger;

        public EntryRepository(IEntryDataContext dataContext, ILogger<EntryRepository> logger)
        {
            m_dataContext = dataContext;
            m_logger = logger;
        }

        public async Task CreateAsync(Entry entity)
        {
            try
            {
                await m_dataContext.CreateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not create Entry");
            }
        }

        public async Task UpdateAsync(Entry entity)
        {
            try
            {
                await m_dataContext.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not update Entry");
            }
        }

        public async Task DeleteAsync(Entry entity)
        {
            try
            {
                await m_dataContext.DeleteAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not delete Entry");
            }
        }

        public async Task<List<Entry>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            try
            {
                return await m_dataContext.SearchAsync(searchTerms);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not search Entry");
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
                m_logger.LogError(e, "Could not search Entry");
            }

            return null;
        }
    }
}