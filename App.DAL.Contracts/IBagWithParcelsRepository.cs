using App.DAL.DTO;
using Base.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Contracts
{
    public interface IBagWithParcelsRepository : IBaseRepository<BagWithParcels>, IBagWithParcelsRepositoryCustom<BagWithParcels>
    {
        void ModifyState(BagWithParcels bag);

        Task<BagWithParcels> FindByBagNumber(string bagNumber);

        Task<Shipment> FindShipment(Guid shipmentId);
        void AddShipmentToBagWithParcels(BagWithParcels bag, Shipment shipment);
    }

    public interface IBagWithParcelsRepositoryCustom<TEntity>
    {
    }
}
