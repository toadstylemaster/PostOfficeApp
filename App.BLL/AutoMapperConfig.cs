using App.BLL.DTO;
using AutoMapper;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        }
    }
}
