dotnet ef database update --project Netgo.Persistence --startup-project Netgo.API --context NetgoDbContext -- --environment Docker
dotnet ef database update --project Netgo.Identity --startup-project Netgo.API --context NetgoIdentityDbContext -- --environment Docker
