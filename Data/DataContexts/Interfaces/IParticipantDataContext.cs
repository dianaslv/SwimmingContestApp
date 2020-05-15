﻿﻿﻿using System;
   using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
   using Core.Models;

   namespace Data.DataContexts.Interfaces
{
    public interface IParticipantDataContext: IDataContext<Participant>
    {
        Task<DbDataAdapter> FillDbDataAdapterWithJoinAsync(Guid taskId);
    }
}