using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4;
using WinterSpine.IdentityServer.Utilities;
using WinterSpine.IdentityServer.ViewModels;
using WinterSpine.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WinterSpine.IdentityServer.Attributes;
using WinterSpine.IdentityServer.Configs;
using Microsoft.AspNetCore.Authorization;
using WinterSpine.IdentityServer.ViewModels.Account;

namespace WinterSpine.IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly AccountService _account;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _interaction = interaction;
            _account = new AccountService(interaction, httpContextAccessor, clientStore);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var vm = await _account.BuildLoginViewModelAsync(returnUrl);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByNameAsync(model.Username);

                if (identityUser != null && await _userManager.CheckPasswordAsync(identityUser, model.Password))
                {
                    AuthenticationProperties props = null;
                    // only set explicit expiration here if persistent. 
                    // otherwise we reply upon expiration configured in cookie middleware.
                    if (AccountOptions.IsAllowRememberLogin && model.IsRememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    await HttpContext.Authentication.SignInAsync(identityUser.Id, identityUser.UserName, props);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }

            var vm = await _account.BuildLoginViewModelAsync(model);
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await _account.BuildLogoutViewModelAsync(logoutId);

            if (vm.IsShowLogoutPrompt == false)
            {
                return await Logout(vm);
            }

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);

            // delete local authentication cookie
            await HttpContext.Authentication.SignOutAsync();

            return View("LoggedOut", vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser(model.UserName)
                {
                    Id = Guid.NewGuid().ToString()
                };

                await _userManager.CreateAsync(identityUser, model.Password);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}