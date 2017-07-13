using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinterSpine.IdentityServer.Models.Clients
{
    public class ClientRedirectUriViewModel
    {
        public int Id { get; set; }
        public string RedirectUri { get; set; }
    }
}
