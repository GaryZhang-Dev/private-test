using AccountService.Domain;
using AccountService.Domain.Models;
using AuthServerClient.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly AccountDbContext _accountContext;
        private readonly IEventService _events;
        public AuthController(AccountDbContext dbContext, IEventService events, IIdentityServerInteractionService interaction)
        {
            _accountContext = dbContext;
            _events = events;
            _interaction = interaction;
        }
        [HttpPost("auth/login/password")]
        public async Task<IActionResult> PasswordLogin([FromBody] LoginRequest request)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
            if (context == null)
            {
                throw new ApplicationException("Invalid context");
            }
            var user = await _accountContext.Set<Users>().FirstOrDefaultAsync();
            string message = "";
            if (user != null)
            {
                message = "登录成功";
            }
            else
            {
                throw new ApplicationException("用户名或密码有误");
            }
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));
            await HttpContext.SignInAsync(new IdentityServerUser(user.Id.ToString()));
            return Ok(message);
        }

        [HttpPost("auth/register-user")]
        public async Task<IActionResult> RegistUser([FromBody] RegistUserRequest request)
        {
            var store_user = await _accountContext.Set<Users>().FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (store_user == null)
            {
                var user = Users.Regits(request.UserName, request.DisplayName, request.Password);
                _accountContext.Add(user);
                _accountContext.SaveChanges();
            }
            return Ok(request.ReturnUrl);
        }

    }
}
