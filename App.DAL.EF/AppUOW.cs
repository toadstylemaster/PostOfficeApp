using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF
{
    public class AppUOW: EFBaseUOW<AppDbContext>, IAppUOW
    {
        private readonly IMapper _mapper;

        private IParcelRepository? _parcels;

        private IBagWithLettersRepository? _bagWithLetters;

        private IBagWithParcelsRepository? _bagWithParcels;

        private IShipmentRepository? _shipments;

        public AppUOW(AppDbContext dataContext, IMapper mapper) : base(dataContext)
        {
            _mapper = mapper;
        }

        public IParcelRepository ParcelRepository => _parcels ??= new ParcelRepository(UowDbContext, new ParcelMapper(_mapper));

        public IBagWithLettersRepository BagWithLettersRepository => _bagWithLetters ??= new BagWithLettersRepository(UowDbContext, new BagWithLettersMapper(_mapper));

        public IBagWithParcelsRepository BagWithParcelsRepository => _bagWithParcels ??= new BagWithParcelsRepository(UowDbContext, new BagWithParcelsMapper(_mapper));

        public IShipmentRepository ShipmentRepository => _shipments ??= new ShipmentRepository(UowDbContext, new ShipmentMapper(_mapper));
    }
}
