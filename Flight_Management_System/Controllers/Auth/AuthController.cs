using Flight.Management.System.API.Configuration;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.API.Services.Auth;
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
        /// <summary>
        /// Handles user login functionality by validating the provided credentials.
        /// </summary>
        /// <param name="model">The login form model containing user credentials (Email and Password).</param>
        /// <returns>
        /// If the provided credentials are valid, an HTTP response with status code 200 (OK) is returned, along with user tokens.
        /// If the provided credentials are invalid or the user does not exist, an HTTP response with status code 401 (Unauthorized) is returned.
        /// If the model state is not valid, an HTTP response with status code 400 (Bad Request) is returned, along with validation errors.
        /// </returns>
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
                        return StatusCode(401, "The username or password is incorrect.");
                    }
                }

            }
            return BadRequest(ModelState);

        }
        #endregion
        #region Register()
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        /// <summary>
        /// Handles user registration by creating a new user with the provided information.
        /// </summary>
        /// <param name="model">The registration form model containing user details.</param>
        /// <returns>
        /// If the registration is successful, an HTTP response with status code 201 (Created) is returned, along with the created user entity.
        /// If the registration fails due to invalid input data, an HTTP response with status code 400 (Bad Request) is returned, along with validation errors.
        /// </returns>
        public async Task<object> Register([FromBody] RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = _userService.CreateAsync(model);

            return Created($"~api/users/{entity.Id}", entity);

        }
        #endregion
    }
}
