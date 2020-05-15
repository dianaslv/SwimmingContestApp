using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;

namespace Networking.Services.Interfaces
{
    public interface IParticipantService
    {
        Task CreateParticipant(Guid id, string text1, int text2);
        Task UpdateParticipant(Guid idParticipant, string text1, string text2);
        Task DeleteParticipant(Guid id);
        Task<List<Participant>> SearchParticipants(List<Tuple<string, string>> searchTerm);
        Task<DbDataAdapter> GetParticipantsDbAdapter(List<Tuple<string, string>> searchTerm);
        Task<DbDataAdapter> GetParticipantsDbAdapterWithJoinAsync(Guid taskId);
    }
}