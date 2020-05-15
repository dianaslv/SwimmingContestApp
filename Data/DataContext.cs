using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class DataContext : IDbDataContext
    {
        private readonly SqlConnection m_connection;

        public DataContext(IConfiguration configuration)
        {
            configuration.GetChildren();
            m_connection = new SqlConnection(configuration.GetConnectionString("Contest"));
        }
        
        public async Task ExecuteNonQueryAsync(SqlCommand command)
        {
            command.Connection = m_connection;
            await m_connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            m_connection.Close();
        }
        
        public async Task<DbDataReader> ExecuteAndGetReaderAsync(SqlCommand command)
        {
            command.Connection = m_connection;
            await m_connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            return reader;
        }

        public async Task<DbDataAdapter> ExecuteAndGetDataAdapterAsync(SqlCommand command)
        {
            command.Connection = m_connection;
            await m_connection.OpenAsync();
            var adapter = new SqlDataAdapter(command);
            m_connection.Close();
            return adapter;
        }

        public void CloseConnection() => m_connection.Close();
    }
}