using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.DataContexts.Interfaces;

namespace Data.DataContexts.Implementations
{
    public class EntryDataContext : IEntryDataContext
    {
        private readonly IEntityDataContext m_entityDataContext;

        public EntryDataContext(IEntityDataContext entityDataContext)
        {
            m_entityDataContext = entityDataContext;
        }


        public async Task CreateAsync(Entry entry)
        {
            var cmd = $"INSERT INTO Entry VALUES ('{entry.Id}','{entry.IdParticipant}','{entry.IdTask}');";
            await m_entityDataContext.CreateAsync(cmd);
        }

        public async Task DeleteAsync(Entry entity)
        {
            await m_entityDataContext.DeleteAsync(entity.Id, "Entry");
        }

        public async Task UpdateAsync(Entry entry)
        {
            var cmd = $"UPDATE Entry " +
                      $"SET IdParticipant = '{entry.IdParticipant}'" +
                      $" IdTask = {entry.IdTask}" +
                      $"WHERE Id = '{entry.Id}'";
            await m_entityDataContext.UpdateAsync(cmd);
        }

        public async Task<List<Entry>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            var result = new List<Entry>();
            DbDataReader reader;
            var cmd = "SELECT * FROM Entry";
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

        private async Task ExtractReaderValues(DbDataReader reader, ICollection<Entry> result)
        {
            while (await reader.ReadAsync())
            {
                result.Add(new Entry
                {
                    Id = reader.GetGuid(0),
                    IdParticipant = reader.GetGuid(1),
                    IdTask = reader.GetGuid(2)
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

        public async Task<DbDataAdapter> FillDbDataAdapterAsync(List<Tuple<string, string>> searchTerms)
        {
            var cmd = "SELECT * FROM Entry";
            if (!searchTerms.Any())
            {
                return await m_entityDataContext.GetDataAdapter(cmd);
            }

            cmd = AgregateWhereValues(searchTerms, cmd);
            return await m_entityDataContext.GetDataAdapter(cmd);
        }
    }
}