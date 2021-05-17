using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthClient.Models
{
    public class RegistUserRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string DisplayName { get; set; }
    }
}
