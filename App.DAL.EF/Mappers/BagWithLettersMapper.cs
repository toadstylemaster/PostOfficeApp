using App.DAL.DTO;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.EF.Mappers
{
    public class BagWithLettersMapper : BaseMapper<BagWithLetters, Domain.BagWithLetters>
    {
        public BagWithLettersMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
