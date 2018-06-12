using System.Security.Claims;
using System.Threading.Tasks;
using Server.Auth;
using Server.Helpers;
using Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Server.Models.AccountViewModels;
using Newtonsoft.Json.Linq;

namespace mvc_auth.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

          var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

          JObject jwtObject = JObject.Parse(jwt);

          ApplicationUser user = await _userManager.FindByNameAsync(identity.Name);
          var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

          jwtObject.Add("is_admin", isAdmin);

          return new OkObjectResult(JsonConvert.SerializeObject(jwtObject));
        }

        [HttpGet("is-admin")]
        public IActionResult isAdmin()
        {
            // var userName = HttpContext.User.IsInRole("Admin");
            // ApplicationUser user = await _userManager.FindByNameAsync(HttpContext.User.userName);
            // return Ok(await _userManager.IsInRoleAsync(user, "Admin"));

            return Ok(HttpContext.User.IsInRole("Admin"));
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}