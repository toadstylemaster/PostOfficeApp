using App.BLL.DTO;
using App.DAL.Contracts;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Contracts.Services
{
    public interface IParcelService : IEntityService<Parcel>, IParcelRepositoryCustom<Parcel>
    {
        Task<bool> RemoveParcelsFromDb(List<App.Public.DTO.v1.Parcel>? bags);

        Task<List<App.Public.DTO.v1.Parcel>?> GetParcelsByShipmentId(Guid? shipmentId);

        App.Public.DTO.v1.Parcel PostParcel(App.Public.DTO.v1.Parcel parcel);

        Task<bool> RemoveParcelFromDb(Guid id);
    }
}
