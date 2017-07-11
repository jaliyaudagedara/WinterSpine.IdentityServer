using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterSpine.IdentityServer.Models.Shared;

namespace WinterSpine.IdentityServer.Models.Clients
{
    public class Client
    {
        public Client()
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
        [Display(Name = "Redirect Uri")]
        public string RedirectUri { get; set; }

        [Display(Name = "Post Logout Redirect Uri")]
        public string PostLogoutRedirectUri { get; set; }

        [Display(Name = "Allowed Scopes")]
        public List<CheckBoxListItem> AllowedScopes { get; set; }
    }
}
