using App.BLL.DTO;
using AutoMapper;
using Base.Contracts.Base;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Mappers
{
    public class ShipmentMapper : BaseMapper<Shipment, DAL.DTO.Shipment>
    {
        public ShipmentMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
