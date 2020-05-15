using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;

namespace Networking.Services.Interfaces
{
    public interface IEntryService
    {
        Task CreateEntry(Guid idParticipant1, Guid idParticipant2);
        Task UpdateEntry(Guid idParticipant1, Guid idParticipant2);
        Task DeleteEntry(Guid id);
        Task<List<Entry>> SearchEntrys(List<Tuple<string, string>> searchTerm);
        Task<DbDataAdapter> GetEntrysDbAdapter(List<Tuple<string, string>> searchTerm);
    }
}