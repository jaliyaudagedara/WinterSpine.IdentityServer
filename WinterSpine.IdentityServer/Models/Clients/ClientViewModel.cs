using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterSpine.IdentityServer.Models.Shared;

namespace WinterSpine.IdentityServer.Models.Clients
{
    public class ClientViewModel
    {
        public ClientViewModel()
        {
            AllowedScopes = new List<CheckBoxListItem>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Redirect Uris")]
        public List<ClientRedirectUriViewModel> RedirectUris { get; set; }

        [Display(Name = "Post Logout Redirect Uris")]
        public List<ClientRedirectUriViewModel> PostLogoutRedirectUris { get; set; }

        [Display(Name = "Allowed Scopes")]
        public List<CheckBoxListItem> AllowedScopes { get; set; }
    }
}
