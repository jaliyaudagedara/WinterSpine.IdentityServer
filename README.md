# IdentityServer based in [IdentityServer4](http://docs.identityserver.io/en/release/) 

IdentityServer based on,
* [OpenID Connect (OIDC)](http://openid.net/connect/)
* [IdentityServer4](http://docs.identityserver.io/en/release/)
* [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) 
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

At this point, we are not aiming for a SAAS (May be in v2). For v1, the idea is providing developers an internal fully fledged IdentityServer which uses OIDC, so they can easily plug in whatever the client applications they want. For instance Admin can CRUD clients and it's config (RedirectUris, The scopes clients can request etc. and there is a lot more), Users and their config etc.

This Repo contains the IdentityServer and a MVC Client for testing.

## Road Map (v1)

* Initial IdentityServer4 Setup for OIDC - **DONE**
* Integrate with ASP.NET Core Identity and EF Core - **DONE**
* Initial Testing Client Application - **Done**
* Configure CI, setup a demo public environment
* Admin Functionalities
    * CRUD IdentityServer Configurations
    * CRUD Clients and their configurations
    * CRUD Users
        * Defining Scopes for Users
* User Functionalities
    * Register (not as a admin as there is a inbuilt admin user)
        * May be send an email to Admin and upon approval, user can move ahead with login and user specific functionalities
    * View/Update/Delete Claims
