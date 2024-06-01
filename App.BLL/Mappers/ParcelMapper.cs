using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers
{
    public class ParcelMapper : BaseMapper<Parcel, DAL.DTO.Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
