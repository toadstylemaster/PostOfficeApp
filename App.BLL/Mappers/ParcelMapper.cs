using App.BLL.DTO;
using AutoMapper;
using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Mappers
{
    public class ParcelMapper : BaseMapper<Parcel, DAL.DTO.Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
