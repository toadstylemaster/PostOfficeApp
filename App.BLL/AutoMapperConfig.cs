using App.BLL.DTO;
using AutoMapper;

namespace App.BLL
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Shipment, DAL.DTO.Shipment>().ReverseMap();
            CreateMap<Parcel, DAL.DTO.Parcel>().ReverseMap();
            CreateMap<BagWithLetters, DAL.DTO.BagWithLetters>().ReverseMap();
            CreateMap<BagWithParcels, DAL.DTO.BagWithParcels>().ReverseMap();
            CreateMap<Bag, DAL.DTO.Bag>().ReverseMap();
        }
    }
}
