using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers
{
    public class BagWithParcelsMapper : BaseMapper<App.Public.DTO.v1.BagWithParcels, BagWithParcels>
    {
        public BagWithParcelsMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
