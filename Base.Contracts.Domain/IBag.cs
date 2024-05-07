using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contracts.Domain
{
    public interface IBag: IBag<string>
    {
    }

    public interface IBag<TKey>
    {
        public TKey BagNumber { get; set; }
    }
}
