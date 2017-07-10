using System.Collections.Generic;

namespace WinterSpine.IdentityServer.ViewModels
{
    public class ConsentInputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool IsRememberConsent { get; set; }
        public string ReturnUrl { get; set; }
    }
}