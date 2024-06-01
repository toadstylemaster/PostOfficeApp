using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.Mappers;
using App.BLL.Services;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;

namespace App.BLL
{
    public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
    {
        private readonly IMapper _mapper;

        private IParcelService? _parcels;

        private IShipmentService? _shipments;

        private IBagWithLettersService? _bagWithLetters;

        private IBagWithParcelsService? _bagWithParcels;

        protected IAppUOW Uow;

        public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
        {
            Uow = uow;
            _mapper = mapper;
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await Uow.SaveChangesAsync();
        }

        public IShipmentService Shipments => _shipments ??= new ShipmentService(Uow.ShipmentRepository, new ShipmentMapper(_mapper), new BagMapper(_mapper));

        public IParcelService Parcels => _parcels ??= new ParcelService(Uow.ParcelRepository, new ParcelMapper(_mapper), new BagWithParcelsMapper(_mapper));

        public IBagWithLettersService BagWithLetters => _bagWithLetters ??= new BagWithLettersService(Uow.BagWithLettersRepository, new BagWithLettersMapper(_mapper), new ShipmentMapper(_mapper), new BagMapper(_mapper));
        public IBagWithParcelsService BagWithParcels => _bagWithParcels ??= new BagWithParcelsService(Uow.BagWithParcelsRepository, new BagWithParcelsMapper(_mapper), new ParcelMapper(_mapper), new ShipmentMapper(_mapper), new BagMapper(_mapper));
    }
}
