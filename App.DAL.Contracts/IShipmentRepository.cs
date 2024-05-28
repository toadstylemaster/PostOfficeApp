﻿using App.DAL.DTO;
using Base.Contracts.DAL;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Contracts
{
    public interface IShipmentRepository : IBaseRepository<Shipment>, IShipmentRepositoryCustom<Shipment>
    {
        void ModifyState(Shipment shipment);


    }

    public interface IShipmentRepositoryCustom<TEntity>
    {
    }


    
}
