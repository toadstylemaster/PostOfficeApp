using App.BLL.DTO;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Mappers
{
    public class BagMapper : BaseMapper<Bag, DAL.DTO.Bag>
    {
        public BagMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
