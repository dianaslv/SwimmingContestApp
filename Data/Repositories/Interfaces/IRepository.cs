using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;

namespace Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> SearchAsync(List<Tuple<string, string>> searchTerms);
        Task<DbDataAdapter> GetDataAdapterAsync(List<Tuple<string, string>> searchTerms);
    }
}