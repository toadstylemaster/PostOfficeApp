using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contracts.Domain
{
    /// <summary>
    ///     default Bag Entity interface
    /// </summary>
    public interface IBag
    {
        public string BagNumber { get; set; }
    }

}
