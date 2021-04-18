using Foudation.CQRS.DDD;
using Foundation.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Domain.Models
{
    public class Users:Entity<Guid>
    {
        public string UserName { get;private set;}

        public string DisplayName { get; private set; }

        public string Password { get; private set; }

        public string Salt { get; private set; }

        public static Users Regits(string userName,string displayName,string passWord) {
            var hash = new PwdHasherHelper();
            var user = new Users {
                UserName = userName,
                DisplayName = string.IsNullOrEmpty(displayName) ? userName : displayName,
                Password = hash.ToHash(passWord),
                Salt = hash.Salt
            };
            return user;
        }
    }
}
