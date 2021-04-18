using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServerClient.Models
{
    public class ApplicationUser:IdentityUser
    {
        public bool IsEnabled { get; set; }

        public string UserId { get; set; }
    }
}
