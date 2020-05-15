using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;
using Data.Repositories.Interfaces;
using Networking.Services.Interfaces;

namespace Networking.Services.Implementations
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository m_repository;

        public ParticipantService(IParticipantRepository repository)
        {
            m_repository = repository;
        }

        public async Task CreateParticipant(Guid id, string text1, int text2)
        {
            var participant = new Participant()
            {
                Id = id,
                Name = text1,
                Age = text2
            };
            await m_repository.CreateAsync(participant);
        }

        public async Task UpdateParticipant(Guid idParticipant, string text1, string text2)
        {
            var Participant = new Participant
            {
                Id = idParticipant,
                Name = text1,
                Age = Int32.Parse(text2)
            };
            await m_repository.UpdateAsync(Participant);
        }

        public async Task DeleteParticipant(Guid id)
        {
            await m_repository.DeleteAsync(new Participant {Id = id});
        }

        public async Task<List<Participant>> SearchParticipants(List<Tuple<string, string>> searchTerm)
        {
            return await m_repository.SearchAsync(searchTerm);
        }

        public async Task<DbDataAdapter> GetParticipantsDbAdapter(List<Tuple<string, string>> searchTerm)
        {
            return await m_repository.GetDataAdapterAsync(searchTerm);
        }

        public async Task<DbDataAdapter> GetParticipantsDbAdapterWithJoinAsync(Guid taskId)
        {
            return await m_repository.GetParticipantsWithTask(taskId);
        }
    }
}