using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinterSpine.IdentityServer.ViewModels.Clients;
using IdentityServer4.EntityFramework.DbContexts;
using WinterSpine.IdentityServer.ViewModels.Shared;
using Microsoft.EntityFrameworkCore;

namespace WinterSpine.IdentityServer.Controllers
{
    public class ClientsController : Controller
    {
        private ConfigurationDbContext _configurationDbContext;

        public ClientsController(ConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        // GET: Clients
        public ActionResult Index()
        {
            List<RegisterClientViewModel> clients = _configurationDbContext.Clients.Select(client => new RegisterClientViewModel()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                RedirectUri = client.RedirectUris.FirstOrDefault().RedirectUri,
                PostLogoutRedirectUri = client.PostLogoutRedirectUris.FirstOrDefault().PostLogoutRedirectUri,
                AllowedScopes = client.AllowedScopes.Select(scope => new CheckBoxListItem()
                {
                    Display = scope.Scope,
                    Value = scope.Scope,
                    IsChecked = true
                }).ToList()
            }).ToList();

            return View(clients);
        }

        // GET: Clients/Details/5
        public ActionResult Details(string id)
        {
            return Edit(id);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            RegisterClientViewModel vm = new RegisterClientViewModel();
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

        // POST: Clients/Create
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

        // GET: Clients/Edit/5
        public ActionResult Edit(string id)
        {
            var client = _configurationDbContext.Clients
                .Include(c => c.RedirectUris)
                .Include(c => c.PostLogoutRedirectUris)
                .Include(c => c.AllowedScopes)
                .FirstOrDefault(c => c.ClientId == id);

            RegisterClientViewModel vm = new RegisterClientViewModel()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                RedirectUri = client.RedirectUris.FirstOrDefault().RedirectUri,
                PostLogoutRedirectUri = client.PostLogoutRedirectUris.FirstOrDefault().PostLogoutRedirectUri,
                AllowedScopes = client.AllowedScopes.Select(scope => new CheckBoxListItem()
                {
                    Display = scope.Scope,
                    Value = scope.Scope,
                    IsChecked = true
                }).ToList()
            };

            return View(vm);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Clients/Delete/5
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