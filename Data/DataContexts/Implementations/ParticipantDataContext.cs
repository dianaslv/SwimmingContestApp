using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Data.DataContexts.Interfaces;

namespace Data.DataContexts.Implementations
{
    public class ParticipantDataContext : IParticipantDataContext
    {
        private readonly IEntityDataContext m_entityDataContext;

        public ParticipantDataContext(IEntityDataContext entityDataContext)
        {
            m_entityDataContext = entityDataContext;
        }


        public async Task CreateAsync(Participant participant)
        {
            var cmd = $"INSERT INTO Participant VALUES ('{participant.Id}','{participant.Name}',{participant.Age});";
            await m_entityDataContext.CreateAsync(cmd);
        }

        public async Task DeleteAsync(Participant entity)
        {
            await m_entityDataContext.DeleteAsync(entity.Id, "Participant");
        }

        public async Task UpdateAsync(Participant participant)
        {
            var cmd = $"UPDATE Participant " +
                      $"SET Name = '{participant.Name}'" +
                      $" Age = {participant.Age}" +
                      $"WHERE Id = '{participant.Id}'";
            await m_entityDataContext.UpdateAsync(cmd);
        }

        public async Task<List<Participant>> SearchAsync(List<Tuple<string, string>> searchTerms)
        {
            var result = new List<Participant>();
            DbDataReader reader;
            var cmd = "SELECT * FROM Participant";
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

        private async Task ExtractReaderValues(DbDataReader reader, ICollection<Participant> result)
        {
            while (await reader.ReadAsync())
            {
                result.Add(new Participant
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
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
            var cmd = "SELECT * FROM Participant";
            if (searchTerms == null || !searchTerms.Any())
            {
                return await m_entityDataContext.GetDataAdapter(cmd);
            }

            cmd = AgregateWhereValues(searchTerms, cmd);
            return await m_entityDataContext.GetDataAdapter(cmd);
        }

        public async Task<DbDataAdapter> FillDbDataAdapterWithJoinAsync(Guid taskId)
        {
           // var cmd = $"SELECT p.*,t.Id as ContestTaskId FROM Participant p JOIN Entry e On e.IdTask = {taskId.ToString()} Join ContestTask t ON t.Id = e.IdTask";
            var cmd = $"select p.* from Participant p  inner join Entry e on p.Id = e.IdParticipant inner join ContestTask t on t.Id = e.IdTask and t.Id='{taskId.ToString()}'";
            Console.WriteLine(cmd);
            return await m_entityDataContext.GetDataAdapter(cmd);
        }
    }
}