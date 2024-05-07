using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers
{
    public class ParcelMapper : BaseMapper<App.Public.DTO.v1.Parcel, Parcel>
    {
        public ParcelMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}
