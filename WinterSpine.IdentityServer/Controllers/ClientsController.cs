using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinterSpine.IdentityServer.Models.Clients;
using IdentityServer4.EntityFramework.DbContexts;
using WinterSpine.IdentityServer.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WinterSpine.IdentityServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        private ConfigurationDbContext _configurationDbContext;

        public ClientsController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public ActionResult Index()
        {
            Dictionary<string, string> claims = new Dictionary<string, string>();

            var c = User.Claims;

            List<ClientViewModel> clients = _configurationDbContext.Clients.Select(client => new ClientViewModel()
            {
                Id = client.Id,
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                RedirectUris = client.RedirectUris.Select(uri => new ClientRedirectUriViewModel()
                {
                    Id = uri.Id,
                    RedirectUri = uri.RedirectUri
                }).ToList(),
                PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(uri => new ClientRedirectUriViewModel()
                {
                    Id = uri.Id,
                    RedirectUri = uri.PostLogoutRedirectUri
                }).ToList(),
                AllowedScopes = client.AllowedScopes.Select(scope => new CheckBoxListItem()
                {
                    Display = scope.Scope,
                    Value = scope.Scope,
                    IsChecked = true
                }).ToList()
            }).ToList();

            return View(clients);
        }

        public ActionResult Details(int id)
        {
            return Edit(id);
        }

        public ActionResult Create()
        {
            ClientViewModel vm = new ClientViewModel();
            vm.AllowedScopes.Add(new CheckBoxListItem()
            {
                Display = "OpenID",
                Value = "openid",
                IsChecked = false
            });
            vm.AllowedScopes.Add(new CheckBoxListItem()
            {
                Display = "Profile",
                Value = "profile",
                IsChecked = false
            });
            vm.AllowedScopes.Add(new CheckBoxListItem()
            {
                Display = "Email",
                Value = "email",
                IsChecked = false
            });

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var client = _configurationDbContext.Clients
                .Include(c => c.RedirectUris)
                .Include(c => c.PostLogoutRedirectUris)
                .Include(c => c.AllowedScopes)
                .FirstOrDefault(c => c.Id == id);

            ClientViewModel vm = new ClientViewModel()
            {
                Id = client.Id,
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                RedirectUris = client.RedirectUris.Select(uri => new ClientRedirectUriViewModel()
                {
                    Id = uri.Id,
                    RedirectUri = uri.RedirectUri
                }).ToList(),
                PostLogoutRedirectUris = client.PostLogoutRedirectUris.Select(uri => new ClientRedirectUriViewModel()
                {
                    Id = uri.Id,
                    RedirectUri = uri.PostLogoutRedirectUri
                }).ToList(),
                AllowedScopes = client.AllowedScopes.Select(scope => new CheckBoxListItem()
                {
                    Display = scope.Scope,
                    Value = scope.Scope,
                    IsChecked = true
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditClient(ClientViewModel vm)
        {
            Client client = _configurationDbContext.Clients.Include(c => c.RedirectUris).Include(c => c.PostLogoutRedirectUris).FirstOrDefault(c => c.Id == vm.Id);
            if (client == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(client, string.Empty, c => c.ClientId, c => c.ClientName))
            {
                try
                {
                    // RedirectUris
                    // removing existing uris which are deleted by client
                    client.RedirectUris.RemoveAll(c => c.Id > 0 && !vm.RedirectUris.Exists(u => u.Id == c.Id));

                    foreach (var item in vm.RedirectUris)
                    {
                        // uri does not exists
                        if (client.RedirectUris.FirstOrDefault(u => u.Id == item.Id) == null)
                        {
                            client.RedirectUris.Add(new ClientRedirectUri() { RedirectUri = item.RedirectUri });
                            continue;
                        }

                        // uri is updated
                        if (client.RedirectUris.FirstOrDefault(u => u.Id == item.Id && u.RedirectUri != item.RedirectUri) != null)
                        {
                            client.RedirectUris.FirstOrDefault(u => u.Id == item.Id).RedirectUri = item.RedirectUri;
                            continue;
                        }
                    }

                    // PostLogoutRedirectUris
                    // removing existing uris which are deleted by client
                    client.PostLogoutRedirectUris.RemoveAll(c => c.Id > 0 && !vm.PostLogoutRedirectUris.Exists(u => u.Id == c.Id));

                    foreach (var item in vm.PostLogoutRedirectUris)
                    {
                        // uri does not exists
                        if (client.PostLogoutRedirectUris.FirstOrDefault(u => u.Id == item.Id) == null)
                        {
                            client.PostLogoutRedirectUris.Add(new ClientPostLogoutRedirectUri() { PostLogoutRedirectUri = item.RedirectUri });
                            continue;
                        }

                        // uri is updated
                        if (client.PostLogoutRedirectUris.FirstOrDefault(u => u.Id == item.Id && u.PostLogoutRedirectUri != item.RedirectUri) != null)
                        {
                            client.PostLogoutRedirectUris.FirstOrDefault(u => u.Id == item.Id).PostLogoutRedirectUri = item.RedirectUri;
                            continue;
                        }
                    }

                    await _configurationDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    // TODO: Log the error
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(client);
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}