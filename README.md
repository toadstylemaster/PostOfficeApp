# PostOfficeApp

#### Creating migrations

~~~bash
dotnet ef migrations --project App.DAL.EF --startup-project WebApp add Initial

dotnet ef database --project App.DAL.EF --startup-project WebApp update

dotnet ef database drop --project App.DAL.EF --startup-project WebApp
~~~

#### API Controllers

~~~bash
cd WebApp
dotnet aspnet-codegenerator controller -name ShipmentController -m App.Domain.Shipment -actions -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~