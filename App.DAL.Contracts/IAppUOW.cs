using Base.Contracts.DAL;

namespace App.DAL.Contracts
{
    public interface IAppUOW: IBaseUOW
    {
        IParcelRepository ParcelRepository { get; }
        IBagWithLettersRepository BagWithLettersRepository { get; }
        IBagWithParcelsRepository BagWithParcelsRepository { get; }
        IShipmentRepository ShipmentRepository { get; }
    }
}
