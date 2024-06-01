using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers
{
    public class BagWithLettersMapper : BaseMapper<App.Public.DTO.v1.BagWithLetters, BagWithLetters>
    {
        public BagWithLettersMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
