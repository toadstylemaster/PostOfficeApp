using App.DAL.DTO;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.EF.Mappers
{
    public class ShipmentMapper : BaseMapper<Shipment, Domain.Shipment>
    {
        public ShipmentMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
