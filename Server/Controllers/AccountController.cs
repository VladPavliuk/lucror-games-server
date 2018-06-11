using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
       // private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
             UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            //IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_emailSender = emailSender;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody]JObject data)
        {
            var user = new ApplicationUser
            {
                UserName = data["email"].ToString(),
                Email = data["email"].ToString(),
                PhoneNumber = data["phoneNumber"].ToString(),
                FirstName = data["firstName"].ToString(),
                LastName = data["lastName"].ToString()
            };
            var result = await _userManager.CreateAsync(user, data["password"].ToString());
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                //await _emailSender.SendEmailConfirmationAsync(data["email"].ToString(), callbackUrl);

                await _signInManager.SignInAsync(user, isPersistent: false);

                _logger.LogInformation("User created a new account with password.");

                return Ok(data);
            }
            return BadRequest(result);
        }
    }
}