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
    public class ParcelMapper : BaseMapper<Parcel, Domain.Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
