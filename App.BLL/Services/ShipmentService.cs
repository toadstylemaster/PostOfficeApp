using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Services
{
    public class ShipmentService : BaseEntityService<Shipment, DAL.DTO.Shipment, IShipmentRepository>, IShipmentService
    {
        public ShipmentService(IShipmentRepository repository, ShipmentMapper mapper) : base(repository, mapper)
        {
        }

    }
}
