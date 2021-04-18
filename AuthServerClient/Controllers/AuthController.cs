using AccountService.Domain;
using AccountService.Domain.Models;
using AuthServerClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly AccountDbContext _accountContext;
        public AuthController(AccountDbContext dbContext)
        {

            _accountContext = dbContext;
        }
        [HttpPost("auth/login/password")]
        public async Task<IActionResult> PasswordLogin([FromBody] LoginRequest request)
        {
            var user = await _accountContext.Set<Users>().FirstOrDefaultAsync();
            string message = "";
            if (user != null)
            {
                message = "登录成功";
            }
            else {
                throw new ApplicationException("用户名或密码有误");
            }

            return Ok(message);
        }

        [HttpPost("auth/register-user")]
        public async Task<IActionResult> RegistUser([FromBody] RegistUserRequest request)
        {
            var store_user = await _accountContext.Set<Users>().FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (store_user == null) {
                var user = Users.Regits(request.UserName, request.DisplayName, request.Password);
                _accountContext.Add(user);
                _accountContext.SaveChanges();
            }
            return Ok(request.ReturnUrl);
        }

    }
}
