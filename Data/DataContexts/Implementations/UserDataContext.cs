using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.DataContexts.Interfaces;

namespace Data.DataContexts.Implementations
{
    public class UserDataContext : IUserDataContext
    {
        private readonly IEntityDataContext m_entityDataContext;

        public UserDataContext(IEntityDataContext entityDataContext)
        {
            m_entityDataContext = entityDataContext;
        }

        public async Task CreateAsync(User entity)
        {
            var cmd = $"INSERT INTO Users VALUES ('{entity.Id}','{entity.Username}','{entity.Password}')";
            await m_entityDataContext.CreateAsync(cmd);
        }

        public async Task DeleteAsync(User entity)
        {
            await m_entityDataContext.DeleteAsync(entity.Id, "users");
        }

        public async Task UpdateAsync(User entity)
        {
            var cmd = $"UPDATE Users SET Username = '{entity.Username}', Password = '{entity.Password}' WHERE id = '{entity.Id}'";
            await m_entityDataContext.UpdateAsync(cmd);
        }

        public async Task<List<User>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            var result = new List<User>();
            DbDataReader reader;
            var cmd = "SELECT * FROM Users";
            if (!searchTerms.Any())
            {
                reader = await m_entityDataContext.SearchAsync(cmd);
                await ExtractReaderValues(reader, result);
                return result;
            }

            cmd = AgregateWhereValues(searchTerms, cmd);
            Console.WriteLine(cmd);
            reader = await m_entityDataContext.SearchAsync(cmd);
            await ExtractReaderValues(reader, result);

            return result;
        }

        private async Task ExtractReaderValues(DbDataReader reader, ICollection<User> result)
        {
            while (await reader.ReadAsync())
            {
                result.Add(new User
                {
                    Id = reader.GetGuid(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                });
            }
            m_entityDataContext.CloseConnection();
        }

        private static string AgregateWhereValues(List<Tuple<string, string>> searchTerms, string cmd)
        {
            cmd += " WHERE ";
            searchTerms.ForEach(t =>
            {
                var (key, value) = t;
                cmd += $"{key} = '{value}' AND ";
            });
            cmd += "Id is not null;";
            return cmd;
        }

        public async Task<DbDataAdapter> FillDbDataAdapterAsync(List<Tuple<string, string>> searchTerms)
        {
            var cmd = "SELECT * FROM Users";
            if (!searchTerms.Any())
            {
                return await m_entityDataContext.GetDataAdapter(cmd);
            }

            cmd = AgregateWhereValues(searchTerms, cmd);
            return await m_entityDataContext.GetDataAdapter(cmd);
        }
    }

}