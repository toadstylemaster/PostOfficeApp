using App.DAL.DTO;
using Base.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Contracts
{
    public interface IShipmentRepository : IBaseRepository<Shipment>, IShipmentRepositoryCustom<Shipment>
    {
    }

    public interface IShipmentRepositoryCustom<TEntity>
    {
    }
}
