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
using Microsoft.Extensions.Logging;
using System.Net;

namespace FurnitureBuildingSolution.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private IEmailService _emailService;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("sendEmails")]
        public IActionResult SendEmails(string apiKey)
        {
            try
            {
                var success = _emailService.SendEmails(apiKey);
                if (success) {
                    return Ok("Emails sent successfully.");
                }
                else
                {
                    return Ok("Some or all emails were not sent successfully.");
                }
            }
            catch (AppException ex)
            {
                _logger.LogError(ex.ToString());
                // return error message if there was an exception
                return StatusCode(500);
            }
        }
    }
}
