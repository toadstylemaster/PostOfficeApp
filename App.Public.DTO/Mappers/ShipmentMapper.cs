using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers
{
    public class ShipmentMapper : BaseMapper<App.Public.DTO.v1.Shipment, Shipment>
    {
        public ShipmentMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
