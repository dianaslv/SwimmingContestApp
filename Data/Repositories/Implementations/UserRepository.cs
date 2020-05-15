using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Xml;
using Core.Models;
using Data.DataContexts.Interfaces;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;


namespace Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDataContext m_dataContext;
        private readonly ILogger<UserRepository> m_logger;

        public UserRepository(IUserDataContext dataContext, ILogger<UserRepository> logger)
        {
            m_dataContext = dataContext;
            m_logger = logger;
        }

        public async Task CreateAsync(User entity)
        {
            try
            {
                await m_dataContext.CreateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, $"Could not create a User : {JsonConvert.SerializeObject(entity, Formatting.Indented)}");
            }
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {
                await m_dataContext.UpdateAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, $"Could not update a User : {JsonConvert.SerializeObject(entity, Formatting.Indented)} ");
            }
        }

        public async Task DeleteAsync(User entity)
        {
            try
            {
                await m_dataContext.DeleteAsync(entity);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, $"Could not delete a User : {JsonConvert.SerializeObject(entity, Formatting.Indented)}");
            }
        }

        public async Task<List<User>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            try
            {
                return await m_dataContext.SearchAsync(searchTerms);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, $"Could not Search for Users with the searchTerms {JsonConvert.SerializeObject(searchTerms, Formatting.Indented)}");
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
                m_logger.LogError(e, $"Could not Search for Users with the searchTerms {JsonConvert.SerializeObject(searchTerms, Formatting.Indented)}");
            }

            return null;
        }
    }
}