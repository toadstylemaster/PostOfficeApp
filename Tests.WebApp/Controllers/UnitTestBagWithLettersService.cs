using App.BLL.Contracts.Services;
using App.BLL.Services;
using App.DAL.EF.Repositories;
using App.DAL.EF;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Tests.WebApp.Controllers
{
    public class UnitTestBagWithLettersService
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IBagWithLettersService _service;
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

        private static MapperConfiguration GetBllMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.BLL.DTO.BagWithLetters, App.DAL.DTO.BagWithLetters>().ReverseMap();
                cfg.CreateMap<App.BLL.DTO.Bag, App.DAL.DTO.Bag>().ReverseMap();
            });
        }

        private static MapperConfiguration GetDalMapperConfiguration()
        {
            return new(cfg =>
            {
                cfg.CreateMap<App.DAL.DTO.BagWithLetters, App.Domain.BagWithLetters>().ReverseMap();
                cfg.CreateMap<App.DAL.DTO.Bag, App.Domain.Bag>().ReverseMap();
            });
        }
    }
}
