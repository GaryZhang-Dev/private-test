﻿using AccountService.Domain;
using AccountService.Domain.Models;
using Api.infrastructure;
using AuthClient.Models;
using Foundation.Common;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly AccountDbContext _accountContext;
        private readonly IEventService _events;
        private readonly IClientStore _clientStore;
        public AuthController(AccountDbContext dbContext, IEventService events, IIdentityServerInteractionService interaction, IClientStore clientStore)
        {
            _accountContext = dbContext;
            _events = events;
            _interaction = interaction;
            _clientStore = clientStore;
        }

       
        [HttpPost("auth/login/password")]
        public async Task<IActionResult> PasswordLogin([FromBody] LoginRequest request)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
            if (context == null)
            {
                throw new ApplicationException("Invalid context");
            }
            var user = await _accountContext.Set<Users>().FirstOrDefaultAsync(x => x.UserName == request.UserName);
            string message = "";
            if (user == null)
            {
                throw new ApplicationException("用户名或密码有误");
            }
            var hashedPwd = new PwdHasherHelper(user.Salt).ToHash(request.Password);

            if (user.Password != hashedPwd)
            {
                throw new ApplicationException("用户名或密码有误");
            }

            message = "登录成功";

            
            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));
            await HttpContext.SignInAsync(new IdentityServerUser(user.Id.ToString())
            {
                DisplayName = user.UserName
            });
            return Ok(new { request.ReturnUrl });
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

        [HttpPost("auth/client")]
        public async Task<IActionResult> GetClient([FromBody] GetClientRequest request)
        {
            var context = await _interaction.GetAuthorizationContextAsync(request.ReturnUrl);
            if (context == null)
            {
                throw new ApplicationException("Invalid context");
            }

            var client = await _clientStore.FindClientByIdAsync(context.Client.ClientId);
            if (client == null)
            {
                throw new ApplicationException("Invalid client");
            }

            if (client.ClientId != ClientIds.AuthWebClient && client.ClientId != ClientIds.AuthWebClientForMobile)
            {
                throw new ApplicationException("Invalid client");
            }

            return Ok(new { context.Client.ClientId });
        }

    }
}
