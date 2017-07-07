# IdentityServer based in [IdentityServer4](http://docs.identityserver.io/en/release/) 

IdentityServer based on,
* [OpenID Connect (OIDC)](http://openid.net/connect/)
* [IdentityServer4](http://docs.identityserver.io/en/release/)
* [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) 
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

This Repo contains the IdentityServer and a MVC Client for testing.

## Database Migrations
```
# Drop Database
dotnet ef database drop -c ApplicationIdentityDbContext

# Initial Migration (Already added to the solution)
dotnet ef migrations add InitialPersistedGrantDb -c PersistedGrantDbContext -o Migrations/PersistedGrantDb
dotnet ef migrations add InitialConfigurationDb -c ConfigurationDbContext -o Migrations/ConfigurationDb
dotnet ef migrations add InitialApplicationIdentityDb -c ApplicationIdentityDbContext -o Migrations/ApplicationIdentityDb

# Update Database Manually
dotnet ef database update -c PersistedGrantDbContext
dotnet ef database update -c ConfigurationDbContext
dotnet ef database update -c ApplicationIdentityDbContext

```