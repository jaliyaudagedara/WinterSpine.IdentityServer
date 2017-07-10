namespace WinterSpine.IdentityServer.ViewModels
{
    public class LoggedOutViewModel
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }

        public bool IsAutomaticRedirectAfterSignOut { get; set; }

        public string LogoutId { get; set; }
        public bool IsTriggerExternalSignout => ExternalAuthenticationScheme != null;
        public string ExternalAuthenticationScheme { get; set; }
    }
}