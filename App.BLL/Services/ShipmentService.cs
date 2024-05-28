using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services
{
    public class ShipmentService : BaseEntityService<Shipment, DAL.DTO.Shipment, IShipmentRepository>, IShipmentService
    {
        private readonly BagMapper _bagMapper;

        public ShipmentService(IShipmentRepository repository, ShipmentMapper mapper, BagMapper bagMapper) : base(repository, mapper)
        {
            _bagMapper = bagMapper;
        }

        public async Task<bool> DeleteShipmentFromDb(Guid shipmentId)
        {
            var shipment = await Repository.FindAsync(shipmentId, true);
            if(shipment == null)
            {
                throw new ArgumentException("Shipment with given id does not exist!");
            }

            await Repository.RemoveAsync(shipment.Id, true);
            return true;
        }

        public async Task<Shipment> GetShipment(Guid id)
        {
            var shipment = await Repository.FindAsync(id, true);
            if (shipment == null)
            {
                throw new Exception("Cant find shipment with given id.");
            }

            return Mapper.Map(shipment)!;
        }

        public async Task<IEnumerable<Shipment>> GetShipments()
        {
            var res = (await Repository.AllAsync(true))
                .Select(x => new Shipment()
                {
                    Id = x.Id,
                    ShipmentNumber = x.ShipmentNumber,
                    Airport = x.Airport,
                    FlightNumber = x.FlightNumber,
                    FlightDate = x.FlightDate,
                    ListOfBags = x.ListOfBags?.Select(x => _bagMapper.Map(x)!).ToList(),
                    IsFinalized = x.IsFinalized,
                })
                .AsEnumerable();
            return res;
        }

        public Shipment PostShipment(Shipment shipment)
        {
            if(shipment == null) 
            {
                throw new ArgumentNullException("Shipment is invalid!");
            }
            return Mapper.Map(Repository.Add(Mapper.Map(shipment)!))!;
        }

        public void ModifyState(Shipment shipment)
        {
            Repository.ModifyState(Mapper.Map(shipment)!);
        }

        public async Task<bool> PutShipment(Guid id, Shipment shipment)
        {
            var shipmentFromDb = await Repository.FindAsync(id, true);
            if (shipmentFromDb == null)
            {
                throw new ArgumentException("Shipment with that id does not exist!");
            }
            if (shipmentFromDb.IsFinalized == true) { throw new InvalidOperationException("Shipment is already finalized!"); }

            shipmentFromDb.ShipmentNumber = shipment.ShipmentNumber;
            shipmentFromDb.Airport = shipment.Airport;
            shipmentFromDb.FlightNumber = shipment.FlightNumber;
            shipmentFromDb.FlightDate = shipment.FlightDate;
            shipmentFromDb.ListOfBags = shipment.ListOfBags?.Select(x => _bagMapper.Map(x)!).ToList();
            shipmentFromDb.IsFinalized = shipment.IsFinalized;


            Repository.Update(shipmentFromDb);
            return true;
        }

        public async Task<bool> PutShipment(Guid id, bool isFinalized)
        {
            var shipmentFromDb = await Repository.FindAsync(id, true);
            if (shipmentFromDb == null)
            {
                throw new ArgumentException("Shipment with given id not found");
            }
            if (isFinalized)
            {
                shipmentFromDb.IsFinalized = true;
            }

            Repository.Update(shipmentFromDb);

            return true;
        }

    }
}
