using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Foundation.Common
{
    public class PwdHasherHelper
    {
        public string Salt { get; }

        public PwdHasherHelper() : this(Guid.NewGuid().ToString("N"))
        {

        }

        public PwdHasherHelper(string _salt) {
            Salt = _salt;
        }

        public string ToHash(string str) {
            using (var sha256 = SHA256.Create()) {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str + Salt));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
