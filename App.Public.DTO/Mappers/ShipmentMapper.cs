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
    public class ShipmentMapper : BaseMapper<App.Public.DTO.v1.Shipment, Shipment>
    {
        public ShipmentMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
