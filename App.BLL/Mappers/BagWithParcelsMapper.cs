using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers
{
    public class BagWithParcelsMapper : BaseMapper<BagWithParcels, DAL.DTO.BagWithParcels>
    {
        public BagWithParcelsMapper(IMapper mapper) : base(mapper)
        {
        }


    }
}
