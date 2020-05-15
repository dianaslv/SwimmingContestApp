using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.DataContexts.Interfaces;

namespace Data.DataContexts.Implementations
{
    public class ContestTaskDataContext : IContestTaskDataContext
    {
        private readonly IEntityDataContext m_entityDataContext;

        public ContestTaskDataContext(IEntityDataContext entityDataContext)
        {
            m_entityDataContext = entityDataContext;
        }

        public async Task CreateAsync(ContestTask contestTask)
        {
            var command = $"INSERT INTO ContestTask VALUES ('{contestTask.Id}','{contestTask.Distance}','{contestTask.Style};";
            await m_entityDataContext.CreateAsync(command);
        }

        public async Task DeleteAsync(ContestTask entity)
        {
            await m_entityDataContext.DeleteAsync(entity.Id, "ContestTask");
        }

        public async Task UpdateAsync(ContestTask contestTask)
        {
            var cmd = $"UPDATE ContestTask " +
                      $"SET Distance = '{contestTask.Distance}'" +
                      $" Style = {contestTask.Style}" +
                      $"WHERE Id = '{contestTask.Id}'";
            await m_entityDataContext.UpdateAsync(cmd);
        }

        public async Task<List<ContestTask>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            var result = new List<ContestTask>();
            DbDataReader reader;
            var cmd = "SELECT * FROM ContestTask";
            if (!searchTerms.Any())
            {
                reader = await m_entityDataContext.SearchAsync(cmd);
                await ExtractReaderValues(reader, result);
                return result;
            }

            cmd = AgregateWhereValues(searchTerms, cmd);
            reader = await m_entityDataContext.SearchAsync(cmd);
            await ExtractReaderValues(reader, result);

            return result;
        }

        public async Task<DbDataAdapter> FillDbDataAdapterAsync(List<Tuple<string, string>> searchTerms)
        {
            var cmd = "SELECT * FROM ContestTask";
            if (searchTerms == null || !searchTerms.Any())
            {
                return await m_entityDataContext.GetDataAdapter(cmd);
            }

            cmd = AgregateWhereValues(searchTerms, cmd);
            return await m_entityDataContext.GetDataAdapter(cmd);
        }


        private async Task ExtractReaderValues(DbDataReader reader, ICollection<ContestTask> result)
        {
            while (await reader.ReadAsync())
            {
                result.Add(new ContestTask
                {
                    Id = reader.GetGuid(0),
                    Distance = (Distance) Enum.Parse(typeof(Distance), reader.GetString(1), true),
                    Style = (Style) Enum.Parse(typeof(Style), reader.GetString(2), true)
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
            cmd += " TRUE;";
            return cmd;
        }
    }
}