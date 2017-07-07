﻿using System;
using System.Collections.Generic;
using System.Linq;
using WinterSpine.IdentityServer.Utilities;

namespace WinterSpine.IdentityServer.ViewModels
{
    public class LoginViewModel : LoginInputModel
    {
        public bool IsAllowRememberLogin { get; set; }
        public bool IsEnableLocalLogin { get; set; }
    }
}