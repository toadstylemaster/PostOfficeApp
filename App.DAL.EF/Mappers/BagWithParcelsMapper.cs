using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers
{
    public class BagWithParcelsMapper : BaseMapper<BagWithParcels, Domain.BagWithParcels>
    {
        public BagWithParcelsMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
