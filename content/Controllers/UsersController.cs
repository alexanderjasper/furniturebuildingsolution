using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using FurnitureBuildingSolution.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FurnitureBuildingSolution.Services;
using FurnitureBuildingSolution.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FurnitureBuildingSolution.ViewModels;
using System.Linq;
using FurnitureBuildingSolution.DataRepresentation;

namespace FurnitureBuildingSolution.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _autoMapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            IMapper autoMapper,
            IOptions<AppSettings> appSettings,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _autoMapper = autoMapper;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            try
            {
                var user = _userService.Authenticate(userDto.EmailAddress, userDto.Password);

                if (user == null)
                {
                    throw new ArgumentException("E-mailaddresse eller brugernavn er forkert.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddYears(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info (without password) and token to store client side
                return Ok(new
                {
                    Id = user.Id,
                    EmailAddress = user.EmailAddress,
                    Role = user.Role,
                    Token = tokenString,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            // map dto to entity
            try
            {
                if (!_userService.UserExists(userDto.EmailAddress))
                {
                    // save 
                    _userService.Create(userDto);
                    return Ok();
                }
                else
                {
                    throw new ArgumentException("Der findes allerede en bruger med den angivne e-mailadresse.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        public IActionResult ForgotPassword([FromBody] UserDto userDto)
        {
            try
            {
                _userService.GeneratePasswordRecoveryKeyAsync(userDto.EmailAddress);
                return Ok(new { message = "Vi har nu afsendt et link til genoprettelse af adgangskode til din e-mailadresse." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            try
            {
                _userService.ChangePassword(model.NewPassword, model.RecoveryKey);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var user = _userService.GetById(userId);
                if (user.Role != Enums.Role.Admin)
                {
                    return Unauthorized(new { message = "Access denied. GetUsers is only allowed for admins." });
                }
                var users = _userService.GetAll().OrderByDescending(u => u.Id).Select(u => _autoMapper.Map<UserListData>(u));
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
