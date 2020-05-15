using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Data.DataContexts.Interfaces
{
    public interface IUserDataContext: IDataContext<User>
    {
    }
}
