using System;
using System.Collections.Generic;
using System.Linq;

namespace WinterSpine.IdentityServer.Models
{
    public class LoginViewModel : LoginInputModel
    {
        public bool IsAllowRememberLogin { get; set; }
        public bool IsEnableLocalLogin { get; set; }
    }
}