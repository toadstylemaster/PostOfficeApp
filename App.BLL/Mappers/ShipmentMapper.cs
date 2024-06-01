using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers
{
    public class ShipmentMapper : BaseMapper<Shipment, DAL.DTO.Shipment>
    {
        public ShipmentMapper(IMapper mapper) : base(mapper)
        {

        }
    }
}
