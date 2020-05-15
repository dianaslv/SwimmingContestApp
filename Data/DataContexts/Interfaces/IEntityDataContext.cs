﻿﻿using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Data.DataContexts.Interfaces
{
    public interface IEntityDataContext
    {
        Task<DbDataReader> SearchAsync(string command);
        Task<DbDataAdapter> GetDataAdapter(string command);
        Task<DbDataAdapter> GetDataAdapter(SqlCommand command);
        Task DeleteAsync(Guid id, string tableName);
        Task CreateAsync(string command);
        Task UpdateAsync(string command);
        Task UpdateAsync(SqlCommand command);
        void CloseConnection();
    }
}