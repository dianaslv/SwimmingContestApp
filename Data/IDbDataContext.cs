using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Data
{
    public interface IDbDataContext
    {
        Task ExecuteNonQueryAsync(SqlCommand command);
        Task<DbDataReader> ExecuteAndGetReaderAsync(SqlCommand command);
        Task<DbDataAdapter> ExecuteAndGetDataAdapterAsync(SqlCommand command);
        void CloseConnection();
    }
}