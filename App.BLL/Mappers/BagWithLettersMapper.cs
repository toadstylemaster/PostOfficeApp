using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers
{
    public class BagWithLettersMapper : BaseMapper<BagWithLetters, DAL.DTO.BagWithLetters>
    {
        public BagWithLettersMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
