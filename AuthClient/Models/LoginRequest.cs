using System;
using System.Collections.Generic;
using System.Text;

namespace AuthClient.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public string Ticket { get; set; }

        public string RandStr { get; set; }
    }

    public class GetClientRequest
    {
        public string ReturnUrl { get; set; }
    }
}
