using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;
using Data.Repositories.Interfaces;
using Networking.Services.Interfaces;

namespace Networking.Services.Implementations
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository m_repository;

        public EntryService(IEntryRepository repository)
        {
            m_repository = repository;
        }

        public async Task CreateEntry(Guid idParticipant1, Guid idParticipant2)
        {
            var Entry = new Entry()
            {
                Id = Guid.NewGuid(),
                IdParticipant = idParticipant1,
                IdTask = idParticipant2
            };
            Console.WriteLine(Entry.Id);
            Console.WriteLine(Entry.IdParticipant);
            Console.WriteLine(Entry.IdTask);
            await m_repository.CreateAsync(Entry);
        }

        public async Task UpdateEntry(Guid idParticipant1, Guid idParticipant2)
        {
            var Entry = new Entry
            {
                Id = Guid.NewGuid(),
                IdParticipant = idParticipant1,
                IdTask = idParticipant2
            };
            await m_repository.UpdateAsync(Entry);
        }

        public async Task DeleteEntry(Guid id)
        {
            await m_repository.DeleteAsync(new Entry {Id = id});
        }

        public async Task<List<Entry>> SearchEntrys(List<Tuple<string, string>> searchTerm)
        {
            return await m_repository.SearchAsync(searchTerm);
        }

        public async Task<DbDataAdapter> GetEntrysDbAdapter(List<Tuple<string, string>> searchTerm)
        {
            return await m_repository.GetDataAdapterAsync(searchTerm);
        }
    }
}