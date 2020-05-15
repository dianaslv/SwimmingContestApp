using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Data.DataContexts.Interfaces;

namespace Data.DataContexts.Implementations
{
    public class EntityDataContext : IEntityDataContext
    {
        private readonly IDbDataContext m_dataContext;

        public EntityDataContext(IDbDataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task<DbDataReader> SearchAsync(string command)
        {
            var cmd = new SqlCommand(command);
            return await m_dataContext.ExecuteAndGetReaderAsync(cmd);
        }

        public async Task<DbDataAdapter> GetDataAdapter(string command)
        {
            var cmd = new SqlCommand(command);
            return await m_dataContext.ExecuteAndGetDataAdapterAsync(cmd);
        }

        public async Task<DbDataAdapter> GetDataAdapter(SqlCommand command)
        {
            return await m_dataContext.ExecuteAndGetDataAdapterAsync(command);
        }

        public async Task DeleteAsync(Guid id, string tableName)
        {
            var cmd = new SqlCommand($"DELETE FROM @tableName WHERE Id = @entityId");
            cmd.Parameters.AddWithValue("@tableName", tableName);
            cmd.Parameters.AddWithValue("@entityId", id);
            await m_dataContext.ExecuteNonQueryAsync(cmd);
        }

        public async Task CreateAsync(string command)
        {
            var cmd = new SqlCommand(command);
            await m_dataContext.ExecuteNonQueryAsync(cmd);
        }

        public async Task UpdateAsync(string command)
        {
            var cmd = new SqlCommand(command);
            await m_dataContext.ExecuteNonQueryAsync(cmd);
        }

        public async Task UpdateAsync(SqlCommand command)
        {
            await m_dataContext.ExecuteNonQueryAsync(command);
        }

        public void CloseConnection() => m_dataContext.CloseConnection();
    }
}