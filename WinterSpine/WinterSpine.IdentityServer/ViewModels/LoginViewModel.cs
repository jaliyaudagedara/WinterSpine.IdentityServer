using System;
using System.Collections.Generic;
using System.Linq;
using WinterSpine.IdentityServer.Utilities;

namespace WinterSpine.IdentityServer.ViewModels
{
    public class LoginViewModel : LoginInputModel
    {
        public bool IsAllowRememberLogin { get; set; }
        public bool IsEnableLocalLogin { get; set; }
        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));
        public bool IsExternalLoginOnly => IsEnableLocalLogin == false && ExternalProviders?.Count() == 1;
        public string ExternalLoginScheme => ExternalProviders?.SingleOrDefault()?.AuthenticationScheme;
    }
}