using System;

public class UnitTestShipmentService
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IDiscountService _service;
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
        var logger = loggerFactory.CreateLogger<DiscountRepository>();
        _service = new DiscountService(new DiscountRepository(_ctx, new DiscountMapper(new Mapper(dalMapperCfg))), new App.BLL.Mappers.DiscountMapper(new Mapper(bllMapperCfg)));
    }
}
