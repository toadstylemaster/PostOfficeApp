﻿using App.BLL.DTO;
using AutoMapper;

namespace App.Public.DTO
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Public.DTO.v1.Shipment, Shipment>().ReverseMap();
            CreateMap<Public.DTO.v1.Parcel, Parcel>().ReverseMap();
            CreateMap<Public.DTO.v1.BagWithLetters, BagWithLetters>().ReverseMap();
            CreateMap<Public.DTO.v1.BagWithParcels, BagWithParcels>().ReverseMap();
            CreateMap<Public.DTO.v1.Bag, Bag>().ReverseMap();
        }

    }
}
