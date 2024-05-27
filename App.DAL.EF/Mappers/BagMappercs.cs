using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers
{
    public class BagMapper : BaseMapper<Bag, Domain.Bag>
    {
        public BagMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
