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
    public class UnitTestParcelsService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IParcelService _service;
        private readonly AppDbContext _ctx;

        public UnitTestParcelsService(ITestOutputHelper testOutputHelper)
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
            var logger = loggerFactory.CreateLogger<ParcelRepository>();
            _service = new ParcelService(new ParcelRepository(_ctx, new App.DAL.EF.Mappers.ParcelMapper(new Mapper(dalMapperCfg)), new App.DAL.EF.Mappers.BagWithParcelsMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.ParcelMapper(new Mapper(bllMapperCfg)), new App.BLL.Mappers.BagWithParcelsMapper(new Mapper(bllMapperCfg)));
        }

        [Fact]
        public async Task TestAddParcel()
        {
            // Arrange
            var parcel = new App.BLL.DTO.Parcel()
            {
                ParcelNumber = "AA123456BB",
                RecipientName = "John Doe",
                DestinationCountry = "EE",
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            var newParcel = _service.PostParcel(parcel);
            await _ctx.SaveChangesAsync();

            // ACT
            var result = await _service.GetParcels();
            var res = await _service.GetParcel(newParcel.Id);

            Assert.Equal(res.Id, result.First().Id);
            Assert.Equal(res.ParcelNumber, result.First().ParcelNumber);
            Assert.NotNull(result);
            Assert.Single(result.ToList());
            var addedBagWithLetters = result.FirstOrDefault(s => s.ParcelNumber == "AA123456BB");
            Assert.NotNull(addedBagWithLetters);
            Assert.Equal("AA123456BB", addedBagWithLetters.ParcelNumber);
            Assert.Equal("John Doe", addedBagWithLetters.RecipientName);
            Assert.Equal("EE", addedBagWithLetters.DestinationCountry);
            Assert.Equal(new decimal(1.001), addedBagWithLetters.Weight);
            Assert.Equal(new decimal(1.01), addedBagWithLetters.Price);
        }

        [Fact]
        public async Task TestRemoveParcel()
        {
            // Arrange
            var parcel = new App.BLL.DTO.Parcel()
            {
                ParcelNumber = "AA123456BB",
                RecipientName = "John Doe",
                DestinationCountry = "EE",
                Weight = new decimal(1.001),
                Price = new decimal(1.01),
            };
            var newParcel = _service.PostParcel(parcel);
            await _ctx.SaveChangesAsync();
            _ctx.ChangeTracker.Clear();


            // ACT
            var result = await _service.RemoveParcelFromDb(newParcel.Id);
            await _ctx.SaveChangesAsync();
            var res = await _service.GetParcels();

            Assert.True(result);
            Assert.Empty(res);
            Assert.Empty(res.ToList());
        }

        private static MapperConfiguration GetBllMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.BLL.DTO.Parcel, App.DAL.DTO.Parcel>().ReverseMap();
            });
        }

        private static MapperConfiguration GetDalMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.DAL.DTO.Parcel, App.Domain.Parcel>().ReverseMap();
            });
        }
    }
}
