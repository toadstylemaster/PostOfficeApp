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
    public class UnitTestBagWithLettersService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IBagWithLettersService _service;
        private readonly IShipmentService _shipmentService;
        private readonly AppDbContext _ctx;

        public UnitTestBagWithLettersService(ITestOutputHelper testOutputHelper)
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
            var logger = loggerFactory.CreateLogger<BagWithLettersRepository>();
            _service = new BagWithLettersService(new BagWithLettersRepository(_ctx, new App.DAL.EF.Mappers.BagWithLettersMapper(new Mapper(dalMapperCfg)), new App.DAL.EF.Mappers.ShipmentMapper(new Mapper(dalMapperCfg)), new App.DAL.EF.Mappers.BagMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.BagWithLettersMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.ShipmentMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.BagMapper(new Mapper(bllMapperCfg)));
            _shipmentService = new ShipmentService(new ShipmentRepository(_ctx, new App.DAL.EF.Mappers.ShipmentMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.ShipmentMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.BagMapper(new Mapper(bllMapperCfg)));
        }

        [Fact]
        public async Task TestAddBagWithLetters()
        {
            // Arrange
            var bagWithLetters = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEF",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            _service.PostBagWithLetters(bagWithLetters);
            await _ctx.SaveChangesAsync();

            // ACT
            var result = await _service.GetBagWithLetters();

            Assert.NotNull(result);
            Assert.Single(result.ToList());
            var addedBagWithLetters = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            Assert.NotNull(addedBagWithLetters);
            Assert.Equal("ABCDEF", addedBagWithLetters.BagNumber);
            Assert.Equal(1, addedBagWithLetters.CountOfLetters);
            Assert.Equal(new decimal(1.001), addedBagWithLetters.Weight);
            Assert.Equal(new decimal(1.01), addedBagWithLetters.Price);
        }

        [Fact]
        public async Task TestGetBagWithLettersAsBags()
        {
            // Arrange
            var bagWithLetters = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEF",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            var newBagWithLetters = _service.PostBagWithLetters(bagWithLetters);
            await _ctx.SaveChangesAsync();


            // ACT
            var result = await _service.GetAllBagWithLettersAsBags();

            Assert.NotNull(result);
            Assert.Single(result.ToList());
            var bag = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            Assert.NotNull(bag);
            Assert.Equal("ABCDEF", bag.BagNumber);
            Assert.Equal(typeof(App.BLL.DTO.Bag), bag.GetType());
        }

        [Fact]
        public async Task TestRemoveBagsWithLetters()
        {
            // Arrange
            var bagWithLetters = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEF",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            var newBagWithLetters = _service.PostBagWithLetters(bagWithLetters);
            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();


            // ACT
            var result = await _service.RemoveBagWithLettersFromDB(newBagWithLetters.Id);
            await _ctx.SaveChangesAsync();
            var res = await _service.GetBagWithLetters();

            Assert.True(result);
            Assert.Empty(res);
            Assert.Empty(res.ToList());
        }

        [Fact]
        public async Task TestAddBagWithLettersToShipment()
        {
            // Arrange
            var bagWithLetters = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEF",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            _service.PostBagWithLetters(bagWithLetters);


            var bagWithLetters2 = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEFG",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            _service.PostBagWithLetters(bagWithLetters2);

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
                Id = bagWithLetters.Id,
                BagNumber = bagWithLetters.BagNumber,
            };            
            var bag2 = new Bag() 
            {
                Id = bagWithLetters2.Id,
                BagNumber = bagWithLetters2.BagNumber,
            };

            var bagList = new List<Bag>() { bag, bag2 };

            // ACT
            var result = await _service.AddBagWithLettersToShipment(bagList, newShipment);
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
        public async Task TestGetBagWithLettersByShipmentId()
        {
            // Arrange
            var bagWithLetters = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEF",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            _service.PostBagWithLetters(bagWithLetters);


            var bagWithLetters2 = new App.BLL.DTO.BagWithLetters
            {
                BagNumber = "ABCDEFG",
                CountOfLetters = 1,
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            _service.PostBagWithLetters(bagWithLetters2);

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
                Id = bagWithLetters.Id,
                BagNumber = bagWithLetters.BagNumber,
            };
            var bag2 = new Bag()
            {
                Id = bagWithLetters2.Id,
                BagNumber = bagWithLetters2.BagNumber,
            };

            var bagList = new List<Bag>() { bag, bag2 };
            await _service.AddBagWithLettersToShipment(bagList, newShipment);
            await _ctx.SaveChangesAsync();

            // ACT
            var result = await _service.GetBagWithLettersByShipmentId(newShipment.Id);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            var addedBag = result.FirstOrDefault(s => s.BagNumber == "ABCDEF");
            var addedBag2 = result.FirstOrDefault(s => s.BagNumber == "ABCDEFG");
            Assert.NotNull(addedBag);
            Assert.Equal("ABCDEF", addedBag.BagNumber);
            Assert.NotNull(addedBag2);
            Assert.Equal("ABCDEFG", addedBag2.BagNumber);
        }

        private static MapperConfiguration GetBllMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.BLL.DTO.BagWithLetters, App.DAL.DTO.BagWithLetters>().ReverseMap();
                cfg.CreateMap<App.BLL.DTO.Bag, App.DAL.DTO.Bag>().ReverseMap();
                cfg.CreateMap<App.BLL.DTO.Shipment, App.DAL.DTO.Shipment>().ReverseMap();
            });
        }

        private static MapperConfiguration GetDalMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.DAL.DTO.BagWithLetters, App.Domain.BagWithLetters>().ReverseMap();
                cfg.CreateMap<App.DAL.DTO.Bag, App.Domain.Bag>().ReverseMap();
                cfg.CreateMap<App.DAL.DTO.Shipment, App.Domain.Shipment>().ReverseMap();
            });
        }
    }
}
