using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.Repositories.Interfaces;
using Networking.Services.Interfaces;

namespace Networking.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository m_repository;

        private const string UsernamePlaceholder = "username";
        private const string PasswordPlaceholder = "password";

        public UserService(IUserRepository repository)
        {
            m_repository = repository;
        }

        public async Task CreateUserAsync(string username, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = password
            };
            await m_repository.CreateAsync(user);
        }

        public async Task<User> LoginUserAsync(string username, string password)
        {
            var users = await m_repository.SearchAsync(new List<Tuple<string, string>>
            {
                new Tuple<string, string>(UsernamePlaceholder, username),
                new Tuple<string, string>(PasswordPlaceholder, password)
            });
            return users.Any() ? users.First() : null;
        }

        public async Task UpdateUserAsync(Guid id, string username, string password)
        {
            var user = new User
            {
                Id = id,
                Username = username,
                Password = password
            };
            await m_repository.UpdateAsync(user);
        }

        public async Task DeleteUser(Guid id)
        {
            var user = new User {Id = id};
            await m_repository.DeleteAsync(user);
        }

        public async Task<List<User>> SearchUsersAsync(List<Tuple<string, string>> searchTerm)
        {
            return await m_repository.SearchAsync(searchTerm);
        }

        public async Task<DbDataAdapter> GetUsersDbAdapterAsync(List<Tuple<string, string>> searchTerm)
        {
            return await m_repository.GetDataAdapterAsync(searchTerm);
        }
    }
}