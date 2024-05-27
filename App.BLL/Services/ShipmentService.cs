using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Mappers;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Domain;

namespace App.BLL.Services
{
    public class ShipmentService : BaseEntityService<Shipment, DAL.DTO.Shipment, IShipmentRepository>, IShipmentService
    {
        public ShipmentService(IShipmentRepository repository, ShipmentMapper mapper) : base(repository, mapper)
        {
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

        public async Task<Public.DTO.v1.Shipment> GetShipment(Guid id)
        {
            var shipment = await Repository.FindAsync(id, true);
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
                ListOfBags = Map(shipment.ListOfBags),
            };

            return shipmentFromDb;
        }

        public async Task<IEnumerable<App.Public.DTO.v1.Shipment>> GetShipments()
        {
            var res = (await Repository.AllAsync(true))
                .Select(x => new App.Public.DTO.v1.Shipment()
                {
                    Id = x.Id,
                    ShipmentNumber = x.ShipmentNumber,
                    Airport = x.Airport,
                    FlightNumber = x.FlightNumber,
                    FlightDate = x.FlightDate,
                    ListOfBags = Map(x.ListOfBags),
                })
                .AsEnumerable();
            return res;
        }

        public App.Public.DTO.v1.Shipment PostShipment(Public.DTO.v1.Shipment shipment)
        {
            var shipmentFromDb = new App.DAL.DTO.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                ListOfBags = Map(shipment.ListOfBags),
            };

            if(shipmentFromDb == null) 
            {
                throw new ArgumentNullException("Shipment is invalid!");
            }
            var dalShipment = Repository.Add(shipmentFromDb);
            var dtoShipment = new App.Public.DTO.v1.Shipment()
            {
                Id = dalShipment.Id,
                ShipmentNumber = dalShipment.ShipmentNumber,
                Airport = dalShipment.Airport,
                FlightNumber = dalShipment.FlightNumber,
                FlightDate = dalShipment.FlightDate,
                ListOfBags = Map(dalShipment.ListOfBags),
            };

            return dtoShipment;
        }

        public App.Public.DTO.v1.Shipment PutBagsToShipment(Guid shipmentId, List<App.Public.DTO.v1.Bag> bags)
        {
            var dalShipment = Repository.FindAsync(shipmentId, true).Result;

            if(dalShipment == null || bags == null)
            {
                throw new ArgumentException("Error: Either shipment with given id was not found or bags were invalid!");
            }
            Repository.Update(dalShipment);

            var dtoShipment = new App.Public.DTO.v1.Shipment()
            {
                Id = dalShipment.Id,
                ShipmentNumber = dalShipment.ShipmentNumber,
                Airport = dalShipment.Airport,
                FlightNumber = dalShipment.FlightNumber,
                FlightDate = dalShipment.FlightDate,
                ListOfBags = Map(dalShipment.ListOfBags),
            };

            return dtoShipment;
        }

        public async Task<bool> PutShipment(Guid id, Public.DTO.v1.Shipment shipment)
        {
            var shipmentFromDb = await Repository.FindAsync(id, true);
            if (shipmentFromDb == null)
            {
                throw new ArgumentException("Shipment with that id does not exist!");
            }
            if (shipmentFromDb.IsFinalized) { throw new InvalidOperationException("Shipment is already finalized!"); }

            shipmentFromDb.ShipmentNumber = shipment.ShipmentNumber;
            shipmentFromDb.Airport = shipment.Airport;
            shipmentFromDb.FlightNumber = shipment.FlightNumber;
            shipmentFromDb.FlightDate = shipment.FlightDate;
            shipmentFromDb.ListOfBags = Map(shipment.ListOfBags);


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

        private List<App.Public.DTO.v1.Bag>? Map(ICollection<DAL.DTO.Bag>? bags)
        {
            if (bags == null)
            {
                return null;
            }
            var bagWithLetters = new App.DAL.DTO.BagWithLetters();
            var bagWithParcels = new App.DAL.DTO.BagWithParcels();
            var finalBags = new List<App.Public.DTO.v1.Bag>();
            foreach (var bag in bags)
            {
                if(bag.GetType() == typeof(DAL.DTO.BagWithParcels))
                {
                    bagWithParcels = (DAL.DTO.BagWithParcels)bag;
                    finalBags.Add(Map(bagWithParcels));
                }
                if(bag.GetType() == typeof(DAL.DTO.BagWithLetters))
                {
                    bagWithLetters = (DAL.DTO.BagWithLetters)bag;
                    finalBags.Add(Map(bagWithLetters));
                }
            }
            return finalBags;
        }        
        
        private ICollection<DAL.DTO.Bag>? Map(ICollection<App.Public.DTO.v1.Bag>? bags)
        {
            if (bags == null)
            {
                return null;
            }
            var bagWithLetters = new App.Public.DTO.v1.BagWithLetters();
            var bagWithParcels = new App.Public.DTO.v1.BagWithParcels();
            var finalBags = new List<DAL.DTO.Bag>();
            foreach (var bag in bags)
            {
                if(bag.GetType() == typeof(App.Public.DTO.v1.BagWithParcels))
                {
                    bagWithParcels = (App.Public.DTO.v1.BagWithParcels)bag;
                    finalBags.Add(Map(bagWithParcels));
                }
                if(bag.GetType() == typeof(App.Public.DTO.v1.BagWithLetters))
                {
                    bagWithLetters = (App.Public.DTO.v1.BagWithLetters)bag;
                    finalBags.Add(Map(bagWithLetters));
                }
            }
            return finalBags;
        }        
        
        private List<DAL.DTO.Bag>? Map(ICollection<App.BLL.DTO.Bag>? bags)
        {
            if (bags == null)
            {
                return null;
            }
            var bagWithLetters = new App.BLL.DTO.BagWithLetters();
            var bagWithParcels = new App.BLL.DTO.BagWithParcels();
            var finalBags = new List<DAL.DTO.Bag>();
            foreach (var bag in bags)
            {
                if(bag.GetType() == typeof(App.BLL.DTO.BagWithParcels))
                {
                    bagWithParcels = (App.BLL.DTO.BagWithParcels)bag;
                    finalBags.Add(Map(bagWithParcels));
                }
                if(bag.GetType() == typeof(App.Public.DTO.v1.BagWithLetters))
                {
                    bagWithLetters = (App.BLL.DTO.BagWithLetters)bag;
                    finalBags.Add(Map(bagWithLetters));
                }
            }
            return finalBags;
        }


        private List<App.Public.DTO.v1.Parcel>? Map(ICollection<DAL.DTO.Parcel>? parcels)
        {
            if (parcels == null)
            {
                return null;
            }
            var res = parcels.Select(parcel =>
            {
                return new App.Public.DTO.v1.Parcel()
                {
                    Id = parcel.Id,
                    ParcelNumber = parcel.ParcelNumber,
                    RecipientName = parcel.RecipientName,
                    DestinationCountry = parcel.DestinationCountry,
                    Price = parcel.Price,
                    Weight = parcel.Weight,
                };
            });
            return res.ToList();
        }        
        
        private List<DAL.DTO.Parcel>? Map(ICollection<App.Public.DTO.v1.Parcel>? parcels)
        {
            if (parcels == null)
            {
                return null;
            }
            var res = parcels.Select(parcel =>
            {
                return new DAL.DTO.Parcel()
                {
                    Id = parcel.Id,
                    ParcelNumber = parcel.ParcelNumber,
                    RecipientName = parcel.RecipientName,
                    DestinationCountry = parcel.DestinationCountry,
                    Price = parcel.Price,
                    Weight = parcel.Weight,
                };
            });
            return res.ToList();
        }        
        
        private List<DAL.DTO.Parcel>? Map(ICollection<App.BLL.DTO.Parcel>? parcels)
        {
            if (parcels == null)
            {
                return null;
            }
            var res = parcels.Select(parcel =>
            {
                return new DAL.DTO.Parcel()
                {
                    Id = parcel.Id,
                    ParcelNumber = parcel.ParcelNumber,
                    RecipientName = parcel.RecipientName,
                    DestinationCountry = parcel.DestinationCountry,
                    Price = parcel.Price,
                    Weight = parcel.Weight,
                };
            });
            return res.ToList();
        }

        private App.Public.DTO.v1.BagWithLetters Map(DAL.DTO.BagWithLetters bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new App.Public.DTO.v1.BagWithLetters()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                CountOfLetters = bag.CountOfLetters,
                Price = bag.Price,
                Weight = bag.Weight,
            };
        }        
        
        private DAL.DTO.BagWithLetters Map(App.Public.DTO.v1.BagWithLetters bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new DAL.DTO.BagWithLetters()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                CountOfLetters = bag.CountOfLetters,
                Price = bag.Price,
                Weight = bag.Weight,
            };
        }        
        
        private DAL.DTO.BagWithLetters Map(App.BLL.DTO.BagWithLetters bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new DAL.DTO.BagWithLetters()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                CountOfLetters = bag.CountOfLetters,
                Price = bag.Price,
                Weight = bag.Weight,
            };
        }

        private App.Public.DTO.v1.BagWithParcels Map(DAL.DTO.BagWithParcels bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new App.Public.DTO.v1.BagWithParcels()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                ListOfParcels = Map(bag.ListOfParcels?.ToList())?.ToList(),
            };
        }

        private DAL.DTO.BagWithParcels Map(App.Public.DTO.v1.BagWithParcels bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new DAL.DTO.BagWithParcels()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                ListOfParcels = Map(bag.ListOfParcels?.ToList())?.ToList(),
            };
        }        
        
        private DAL.DTO.BagWithParcels Map(App.BLL.DTO.BagWithParcels bag)
        {
            if (bag == null)
            {
                throw new ArgumentException("No bag provided!");
            }

            return new DAL.DTO.BagWithParcels()
            {
                Id = bag.Id,
                BagNumber = bag.BagNumber,
                ListOfParcels = Map(bag.ListOfParcels?.ToList())?.ToList(),
            };
        }

        private App.Public.DTO.v1.Shipment? Map(DAL.DTO.Shipment? shipment)
        {
            if (shipment == null)
            {
                return null;
            }

            return new App.Public.DTO.v1.Shipment()
            {
                Id = shipment.Id,
                ShipmentNumber = shipment.ShipmentNumber,
                Airport = shipment.Airport,
                FlightNumber = shipment.FlightNumber,
                FlightDate = shipment.FlightDate,
                IsFinalized = shipment.IsFinalized,
            };
        }

    }
}
