namespace WinterSpine.IdentityServer.Models
{
    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsEmphasize { get; set; }
        public bool IsRequired { get; set; }
        public bool IsChecked { get; set; }
    }
}
