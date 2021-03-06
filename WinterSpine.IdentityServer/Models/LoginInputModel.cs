﻿using System.ComponentModel.DataAnnotations;

namespace WinterSpine.IdentityServer.Models
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsRememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}