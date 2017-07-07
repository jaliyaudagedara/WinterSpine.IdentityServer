using System;

namespace WinterSpine.IdentityServer.Configs
{
    public class AccountOptions
    {
        public static bool IsAllowLocalLogin = true;
        public static bool IsAllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static bool IsShowLogoutPrompt = true;
        public static bool IsAutomaticRedirectAfterSignOut = false;
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
