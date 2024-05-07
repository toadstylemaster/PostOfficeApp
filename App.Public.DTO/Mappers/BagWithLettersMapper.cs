using App.BLL.DTO;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Public.DTO.Mappers
{
    public class BagWithLettersMapper : BaseMapper<App.Public.DTO.v1.BagWithLetters, BagWithLetters>
    {
        public BagWithLettersMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
