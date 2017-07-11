using System.Collections.Generic;

namespace WinterSpine.IdentityServer.Models
{
    public class ConsentInputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool IsRememberConsent { get; set; }
        public string ReturnUrl { get; set; }
    }
}