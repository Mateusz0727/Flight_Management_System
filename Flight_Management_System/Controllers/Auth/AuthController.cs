using Flight.Management.System.API.Configuration;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.API.Services;
using Flight.Management.System.API.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Management.System.API.Controllers.Auth
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        public AuthController(AuthService authService, UserService userService)
        {
            _userService = userService;
            _authService = authService;
        }
        #region Login()
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<UserTokens>> Login([FromBody] LoginFormModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _userService.GetByEmailAsync(model.Email);
                if (entity != null)
                {
                    var result = _authService.Login(model, entity);
                    if (result)
                    {
                        return Ok(_authService.CreateToken(entity));
                    }
                    else
                    {
                        return BadRequest($"wrong password");
                    }
                }
            }

            return StatusCode(401, "[[[Nazwa użytkownika lub hasło są nieprawidłowe.]]]");
        }
        #endregion
        #region Register()
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<object> Register([FromBody] RegisterFormModel model)
        {

            var entity = _userService.CreateAsync(model);

            return Created($"~api/users/{entity.Id}", entity);

        }
        #endregion
    }
}
