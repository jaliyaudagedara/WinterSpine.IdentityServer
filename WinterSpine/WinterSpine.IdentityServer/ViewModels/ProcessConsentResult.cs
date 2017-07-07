namespace WinterSpine.IdentityServer.ViewModels
{
    public class ProcessConsentResult
    {
        public bool IsRedirect => RedirectUri != null;
        public string RedirectUri { get; set; }
        public bool IsShowView => ViewModel != null;
        public ConsentViewModel ViewModel { get; set; }
        public bool HasValidationError => ValidationError != null;
        public string ValidationError { get; set; }
    }
}
