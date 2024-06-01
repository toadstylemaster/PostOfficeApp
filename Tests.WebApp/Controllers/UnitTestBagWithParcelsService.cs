using App.BLL.Contracts.Services;
using App.BLL.DTO;
using App.BLL.Services;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.WebApp.Controllers
{
    public class UnitTestBagWithParcelsService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IBagWithParcelsService _service;
        private readonly IShipmentService _shipmentService;
        private readonly AppDbContext _ctx;

        public UnitTestBagWithParcelsService(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            //set up mock db - inmemory
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _ctx = new AppDbContext(optionsBuilder.Options);

            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();

            _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _ctx.ChangeTracker.AutoDetectChangesEnabled = false;

            var dalMapperCfg = GetDalMapperConfiguration();
            var bllMapperCfg = GetBllMapperConfiguration();

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<BagWithParcelsRepository>();
            _service = new BagWithParcelsService(new BagWithParcelsRepository(_ctx, new App.DAL.EF.Mappers.BagWithParcelsMapper(new Mapper(dalMapperCfg)), new App.DAL.EF.Mappers.ShipmentMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.BagWithParcelsMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.ParcelMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.ShipmentMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.BagMapper(new Mapper(bllMapperCfg)));
            _shipmentService = new ShipmentService(new ShipmentRepository(_ctx, new App.DAL.EF.Mappers.ShipmentMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.ShipmentMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.BagMapper(new Mapper(bllMapperCfg)));
        }

        [Fact]
        public async Task TestAddBagWithParcels()
        {
            // Arrange
            var bagWithParcels = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEF"
            };
            _service.PostBagWithParcels(bagWithParcels);
            await _ctx.SaveChangesAsync();

            // ACT
            var result = await _service.GetBagWithParcels();

            Assert.NotNull(result);
            Assert.Single(result.ToList());
            var addedBagWithParcels = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            Assert.NotNull(addedBagWithParcels);
            Assert.Equal("ABCDEF", addedBagWithParcels.BagNumber);
        }

        [Fact]
        public async Task TestGetBagWithParcelsAsBags()
        {
            // Arrange
            var bagWithParcels = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEF",
            };
            var newBagWithParcels = _service.PostBagWithParcels(bagWithParcels);
            await _ctx.SaveChangesAsync();


            // ACT
            var result = await _service.GetAllBagWithParcelsAsBags();

            Assert.NotNull(result);
            Assert.Single(result.ToList());
            var bag = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            Assert.NotNull(bag);
            Assert.Equal("ABCDEF", bag.BagNumber);
            Assert.Equal(typeof(App.BLL.DTO.Bag), bag.GetType());
        }

        [Fact]
        public async Task TestAddBagWithParcelsToShipment()
        {
            // Arrange
            var bagWithParcels = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEF",
            };
            _service.PostBagWithParcels(bagWithParcels);


            var bagWithParcels2 = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEFG",
            };
            _service.PostBagWithParcels(bagWithParcels2);

            var shipment = new App.BLL.DTO.Shipment
            {
                ShipmentNumber = "AAA-CCCCCC",
                Airport = "HEL",
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
            };
            var newShipment = _shipmentService.PostShipment(shipment);

            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();

            var bag = new Bag()
            {
                Id = bagWithParcels.Id,
                BagNumber = bagWithParcels.BagNumber,
            };
            var bag2 = new Bag()
            {
                Id = bagWithParcels2.Id,
                BagNumber = bagWithParcels2.BagNumber,
            };

            var bagList = new List<Bag>() { bag, bag2 };

            // ACT
            var result = await _service.AddBagWithParcelsToShipment(bagList, newShipment);
            await _ctx.SaveChangesAsync();
            var res = await _shipmentService.GetShipment(newShipment.Id);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            var addedBag = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            var addedBag2 = result.FirstOrDefault(s => s.BagNumber == "ABCDEFG");
            Assert.NotNull(addedBag);
            Assert.Equal("ABCDEF", addedBag.BagNumber);
            Assert.NotNull(addedBag2);
            Assert.Equal("ABCDEFG", addedBag2.BagNumber);
            Assert.Equal(2, res.ListOfBags!.Count());
        }

        [Fact]
        public async Task TestGetBagWithParcelsByShipmentId()
        {
            // Arrange
            var bagWithParcels = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEF",
            };
            _service.PostBagWithParcels(bagWithParcels);


            var bagWithParcels2 = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEFG",
            };
            _service.PostBagWithParcels(bagWithParcels2);

            var shipment = new App.BLL.DTO.Shipment
            {
                ShipmentNumber = "AAA-CCCCCC",
                Airport = "HEL",
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
            };
            var newShipment = _shipmentService.PostShipment(shipment);

            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();

            var bag = new Bag()
            {
                Id = bagWithParcels.Id,
                BagNumber = bagWithParcels.BagNumber,
            };
            var bag2 = new Bag()
            {
                Id = bagWithParcels2.Id,
                BagNumber = bagWithParcels2.BagNumber,
            };

            var bagList = new List<Bag>() { bag, bag2 };
            await _service.AddBagWithParcelsToShipment(bagList, newShipment);
            await _ctx.SaveChangesAsync();

            // ACT
            var result = await _service.GetBagWithParcelsByShipmentId(newShipment.Id);
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            var addedBag = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            var addedBag2 = result.FirstOrDefault(s => s.BagNumber == "ABCDEFG");
            Assert.NotNull(addedBag);
            Assert.Equal("ABCDEF", addedBag.BagNumber);
            Assert.NotNull(addedBag2);
            Assert.Equal("ABCDEFG", addedBag2.BagNumber);
        }

        [Fact]
        public async Task TestRemoveBagsWithParcels()
        {
            // Arrange
            var bagWithParcels = new App.BLL.DTO.BagWithParcels
            {
                BagNumber = "ABCDEF"
            };
            var newBagWithParcels = _service.PostBagWithParcels(bagWithParcels);
            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();


            // ACT
            var result = await _service.RemoveBagWithParcelsFromDb(newBagWithParcels.Id);
            await _ctx.SaveChangesAsync();
            var res = await _service.GetBagWithParcels();

            Assert.True(result);
            Assert.Empty(res);
            Assert.Empty(res.ToList());
        }

        private static MapperConfiguration GetBllMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.BLL.DTO.BagWithParcels, App.DAL.DTO.BagWithParcels>().ReverseMap();
                cfg.CreateMap<App.BLL.DTO.Bag, App.DAL.DTO.Bag>().ReverseMap();
                cfg.CreateMap<App.BLL.DTO.Shipment, App.DAL.DTO.Shipment>().ReverseMap();
            });
        }

        private static MapperConfiguration GetDalMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.DAL.DTO.BagWithParcels, App.Domain.BagWithParcels>().ReverseMap();
                cfg.CreateMap<App.DAL.DTO.Bag, App.Domain.Bag>().ReverseMap();
                cfg.CreateMap<App.DAL.DTO.Shipment, App.Domain.Shipment>().ReverseMap();
            });
        }
    }
}
