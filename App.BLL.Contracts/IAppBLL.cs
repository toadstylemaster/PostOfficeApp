using App.BLL.Contracts.Services;
using Base.Contracts.BLL;

namespace App.BLL.Contracts
{
    public interface IAppBLL : IBaseBLL
    {
        IShipmentService Shipments { get; }
        IParcelService Parcels { get; }
        IBagWithLettersService BagWithLetters { get; }
        IBagWithParcelsService BagWithParcels { get; }
    }
}
