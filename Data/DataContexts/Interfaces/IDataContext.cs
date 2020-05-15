﻿﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;

namespace Data.DataContexts.Interfaces
{
    public interface IDataContext<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        Task<List<T>> SearchAsync(List<Tuple<string, string>> searchTerms);
        Task<DbDataAdapter> FillDbDataAdapterAsync(List<Tuple<string, string>> searchTerms);
    }
}