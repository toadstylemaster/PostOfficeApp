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
    public class UnitTestBagWithParcelsService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IBagWithParcelsService _service;
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
            });
        }

        private static MapperConfiguration GetDalMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.DAL.DTO.BagWithParcels, App.Domain.BagWithParcels>().ReverseMap();
                cfg.CreateMap<App.DAL.DTO.Bag, App.Domain.Bag>().ReverseMap();
            });
        }
    }
}
