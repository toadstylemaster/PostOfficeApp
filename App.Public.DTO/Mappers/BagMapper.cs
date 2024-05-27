using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers
{
    public class BagMapper : BaseMapper<App.Public.DTO.v1.Bag, Bag>
    {
        public BagMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
