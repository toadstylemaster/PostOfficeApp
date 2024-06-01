using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers
{
    public class BagWithLettersMapper : BaseMapper<BagWithLetters, Domain.BagWithLetters>
    {
        public BagWithLettersMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
