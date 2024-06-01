using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers
{
    public class ShipmentMapper : BaseMapper<Shipment, Domain.Shipment>
    {
        public ShipmentMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
