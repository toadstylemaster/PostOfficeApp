using App.BLL.Contracts.Services;
using App.BLL.Services;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.WebApp.Controllers
{
    public class UnitTestShipmentService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IShipmentService _service;
        private readonly AppDbContext _ctx;

        public UnitTestShipmentService(ITestOutputHelper testOutputHelper)
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
            var logger = loggerFactory.CreateLogger<ShipmentRepository>();
            _service = new ShipmentService(new ShipmentRepository(_ctx, new App.DAL.EF.Mappers.ShipmentMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.ShipmentMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.BagMapper(new Mapper(bllMapperCfg)));
        }

        [Fact]
        public async Task TestAddShipment()
        {
            // Arrange
            var shipment = new App.BLL.DTO.Shipment
            {
                ShipmentNumber = "AAA-CCCCCC",
                Airport = "HEL",
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
            };
            var newShipment = _service.PostShipment(shipment);

            await _ctx.SaveChangesAsync();

            // ACT
            var result = await _service.GetShipments();
            var res2 = await _service.GetShipment(newShipment.Id);

            // ASSERT
            Assert.Equal(result.First().Id, res2.Id);
            Assert.Equal(result.First().ShipmentNumber, res2.ShipmentNumber);
            Assert.NotNull(result);
            Assert.Single(result.ToList());
            var addedShipment = result.FirstOrDefault(s => s.ShipmentNumber == "AAA-CCCCCC");
            Assert.NotNull(addedShipment);
            Assert.Equal("AAA-CCCCCC", addedShipment.ShipmentNumber);
            Assert.Equal("HEL", addedShipment.Airport);
            Assert.Equal("AB1234", addedShipment.FlightNumber);
            Assert.False(addedShipment.IsFinalized);
        }

        [Fact]
        public async Task TestDeleteShipment()
        {
            // Arrange
            var shipment = new App.BLL.DTO.Shipment
            {
                ShipmentNumber = "AAA-CCCCCC",
                Airport = "HEL",
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
            };
            var newShipmentId = _service.PostShipment(shipment).Id;
            Console.WriteLine(shipment.ShipmentNumber);

            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();

            // ACT
            var res = await _service.DeleteShipmentFromDb(newShipmentId);
            await _ctx.SaveChangesAsync();
            var result = await _service.GetShipments();

            // ASSERT
            Assert.True(res);
            Assert.Empty(result.ToList());
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestFinalizeShipment()
        {
            // Arrange
            var shipment = new App.BLL.DTO.Shipment
            {
                ShipmentNumber = "AAA-CCCCCC",
                Airport = "HEL",
                FlightNumber = "AB1234",
                FlightDate = DateTime.Now,
                IsFinalized = false,
            };
            var newShipmentId = _service.PostShipment(shipment).Id;
            Console.WriteLine(shipment.ShipmentNumber);

            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();

            // ACT
            var res = await _service.FinalizeShipment(newShipmentId);
            await _ctx.SaveChangesAsync();
            var result = await _service.GetShipments();

            // ASSERT
            Assert.True(res);
            Assert.True(result.First().IsFinalized);
        }

        private static MapperConfiguration GetBllMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.BLL.DTO.Shipment, App.DAL.DTO.Shipment>().ReverseMap();
            });
        }

        private static MapperConfiguration GetDalMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.DAL.DTO.Shipment, App.Domain.Shipment>().ReverseMap();
            });
        }
    }
}