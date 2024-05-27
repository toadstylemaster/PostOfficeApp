using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Contracts.Services
{
    public interface IBagWithParcelsService : IEntityService<BagWithParcels>, IBagWithParcelsRepositoryCustom<BagWithParcels>
    {
        Task<IEnumerable<App.Public.DTO.v1.BagWithParcels>> GetBagWithParcelsByShipmentId(Guid? shipmentId);
        App.Public.DTO.v1.BagWithParcels PutParcelsToBag(Guid bagWithParcelsId, List<App.Public.DTO.v1.Parcel> parcels);
        Task<IEnumerable<App.Public.DTO.v1.BagWithParcels>> GetBagWithParcels();

        App.Public.DTO.v1.BagWithParcels PostBagWithParcels(App.Public.DTO.v1.BagWithParcels bagWithParcels);

        Task<bool> RemoveBagWithParcelsFromDb(Guid id);

        Task<bool> RemoveBagsWithParcelsFromDb(List<App.Public.DTO.v1.BagWithParcels>? bags);
    }
}
