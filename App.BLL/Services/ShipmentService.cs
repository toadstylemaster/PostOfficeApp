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
        public ShipmentService(IShipmentRepository repository, ShipmentMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<Public.DTO.v1.Shipment> GetShipment(Guid id)
        {
            var shipment = await Repository.FindAsync(id);
            if (shipment == null)
            {
                throw new Exception("Cant find shipment with given id.");
            }

            var shipmentFromDb = new App.Public.DTO.v1.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                ListOfBags = shipment.ListOfBags,
            };

            return shipmentFromDb;
        }

        public async Task<IEnumerable<App.Public.DTO.v1.Shipment>> GetShipments()
        {
            var res = (await Repository.AllAsync())
                .Select(x => new App.Public.DTO.v1.Shipment()
                {
                    Id = x.Id,
                    ShipmentNumber = x.ShipmentNumber,
                    Airport = x.Airport,
                    FlightNumber = x.FlightNumber,
                    FlightDate = x.FlightDate,
                    ListOfBags = x.ListOfBags,
                })
                .AsEnumerable();
            return res;
        }

        public void PostShipment(Public.DTO.v1.Shipment shipment)
        {
            var shipmentFromDb = new App.DAL.DTO.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                ListOfBags = shipment.ListOfBags,
            };

            if(shipmentFromDb != null) 
            {
                Repository.Add(shipmentFromDb);
                return;
            }
            throw new ArgumentException("Given shipment is invalid");

            
        }

        public async Task<bool> PutShipment(Guid id, Public.DTO.v1.Shipment shipment)
        {
            var shipmentFromDb = await Repository.FindAsync(id);
            if (shipmentFromDb == null)
            {
                throw new ArgumentException("Shipment with that id does not exist!");
            }
            if (shipmentFromDb.IsFinalized) { throw new InvalidOperationException("Shipment is already finalized!"); }

            shipmentFromDb.ShipmentNumber = shipment.ShipmentNumber;
            shipmentFromDb.Airport = shipment.Airport;
            shipmentFromDb.FlightNumber = shipment.FlightNumber;
            shipmentFromDb.FlightDate = shipment.FlightDate;
            shipmentFromDb.ListOfBags = shipment.ListOfBags;


            Repository.Update(shipmentFromDb);
            return true;
        }

        public async Task<bool> PutShipment(Guid id, bool isFinalized)
        {
            var shipmentFromDb = await Repository.FindAsync(id);
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
