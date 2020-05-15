using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;

namespace Networking.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(string username, string password);
        Task<User> LoginUserAsync(string username, string password);
        Task UpdateUserAsync(Guid id, string username, string password);
        Task DeleteUser(Guid id);
        Task<List<User>> SearchUsersAsync(List<Tuple<string, string>> searchTerm);
        Task<DbDataAdapter> GetUsersDbAdapterAsync(List<Tuple<string, string>> searchTerm);
    }
}