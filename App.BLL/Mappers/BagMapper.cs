using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers
{
    public class BagMapper : BaseMapper<Bag, DAL.DTO.Bag>
    {
        public BagMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
