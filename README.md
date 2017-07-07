```
dotnet ef database drop -c ApplicationIdentityDbContext

dotnet ef migrations add InitialPersistedGrantDb -c PersistedGrantDbContext -o Migrations/PersistedGrantDb
dotnet ef migrations add InitialConfigurationDb -c ConfigurationDbContext -o Migrations/ConfigurationDb
dotnet ef migrations add InitialApplicationIdentityDb -c ApplicationIdentityDbContext -o Migrations/ApplicationIdentityDb

dotnet ef database update -c PersistedGrantDbContext
dotnet ef database update -c ConfigurationDbContext
dotnet ef database update -c ApplicationIdentityDbContext

```