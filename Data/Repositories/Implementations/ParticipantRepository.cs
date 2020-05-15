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
    public class ParticipantRepository : IParticipantRepository
    {
         private readonly IParticipantDataContext m_dataContext;
        private readonly ILogger<ParticipantRepository> m_logger;

        public ParticipantRepository(IParticipantDataContext dataContext, ILogger<ParticipantRepository> logger)
        {
            m_dataContext = dataContext;
            m_logger = logger;
        }

        public async Task CreateAsync(Participant entity)
        {
            try
            {
                await m_dataContext.CreateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not create Participant");
            }
        }

        public async Task UpdateAsync(Participant entity)
        {
            try
            {
                await m_dataContext.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not update Participant");
            }
        }

        public async Task DeleteAsync(Participant entity)
        {
            try
            {
                await m_dataContext.DeleteAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not delete Participant");
            }
        }

        public async Task<List<Participant>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            try
            {
                return await m_dataContext.SearchAsync(searchTerms);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not search Participant");
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
                m_logger.LogError(e, "Could not search Participant");
            }

            return null;
        }

        public async Task<DbDataAdapter> GetParticipantsWithTask(Guid taskId)
        {
            try
            {
                return await m_dataContext.FillDbDataAdapterWithJoinAsync(taskId);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, "Could not find Participants");
            }

            return null;
        }
    }
}