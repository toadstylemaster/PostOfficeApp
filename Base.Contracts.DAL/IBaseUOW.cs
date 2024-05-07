using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contracts.DAL
{
    public interface IBaseUOW
    {
        Task<int> SaveChangesAsync();
    }
}
