using App.DAL.DTO;
using AutoMapper;

namespace App.DAL.EF
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Parcel, Domain.Parcel>().ReverseMap();
            CreateMap<BagWithLetters, Domain.BagWithLetters>().ReverseMap();
            CreateMap<BagWithParcels, Domain.BagWithParcels>().ReverseMap();
            CreateMap<Shipment, Domain.Shipment>().ReverseMap();
            CreateMap<Bag, Domain.Bag>().ReverseMap();
        }
    }
}
