using App.BLL.DTO;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Public.DTO.Mappers
{
    public class BagWithParcelsMapper : BaseMapper<App.Public.DTO.v1.BagWithParcels, BagWithParcels>
    {
        public BagWithParcelsMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
