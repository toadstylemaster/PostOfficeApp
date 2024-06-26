﻿using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;

namespace App.BLL.Contracts.Services
{
    public interface IBagWithParcelsService : IEntityService<BagWithParcels>, IBagWithParcelsRepositoryCustom<BagWithParcels>
    {
        Task<IEnumerable<BagWithParcels>> GetBagWithParcelsByShipmentId(Guid shipmentId);

        Task<IEnumerable<Bag>> AddBagWithParcelsToShipment(List<Bag> bags, Shipment shipmentId);
        Task<IEnumerable<BagWithParcels>> GetBagWithParcels();

        BagWithParcels PostBagWithParcels(BagWithParcels bagWithParcels);

        Task<bool> RemoveBagWithParcelsFromDb(Guid id);

        Task<IEnumerable<Bag>> GetAllBagWithParcelsAsBags();
    }
}
