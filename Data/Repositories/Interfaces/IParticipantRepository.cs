using System;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Models;
using Data.DataContexts.Interfaces;

namespace Data.Repositories.Interfaces
{
    public interface IParticipantRepository:IRepository<Participant>
    {
        Task<DbDataAdapter> GetParticipantsWithTask(Guid taskId);
    }
}