using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterSpine.IdentityServer.ViewModels.Shared;

namespace WinterSpine.IdentityServer.ViewModels.Clients
{
    public class RegisterClientViewModel
    {
        public RegisterClientViewModel()
        {
            AllowedScopes = new List<CheckBoxListItem>();
        }

        [Required]
        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Redirect Uri")]
        public string RedirectUri { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Post Logout Redirect Uri")]
        public string PostLogoutRedirectUri { get; set; }

        [Display(Name = "Allowed Scopes")]
        public List<CheckBoxListItem> AllowedScopes { get; set; }
    }
}
