using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FurnitureBuildingSolution.Dtos;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Repositories;
using FurnitureBuildingSolution.ViewModels;
using Microsoft.Extensions.Options;
using RazorLight;

namespace FurnitureBuildingSolution.Services
{
    public interface IUserService
    {
        UserDto Authenticate(string emailAddress, string password);
        UserDto GetById(int id);
        IEnumerable<User> GetAll();
        UserDto Create(UserDto user);
        void Delete(int id);
        Task<Guid> GeneratePasswordRecoveryKeyAsync(string emailAddress);
        void ChangePassword(string newPassword, string recoveryKey);
        bool UserExists(string emailAddress);
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private IUserRepository _userRepository;
        private readonly IMapper _autoMapper;
        private IEmailService _emailService;
        private readonly IRazorLightEngine _razorEngine;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, IMapper autoMapper, IEmailService emailService, IRazorLightEngine razorEngine)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _autoMapper = autoMapper;
            _emailService = emailService;
            _razorEngine = razorEngine;
        }

        public UserDto Authenticate(string emailAddress, string password)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.GetByEmailAddress(emailAddress);

            // check if emailAddress exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return _autoMapper.Map<UserDto>(user);
        }

        public UserDto GetById(int id)
        {
            var user = _userRepository.GetById(id);
            return _autoMapper.Map<UserDto>(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public UserDto Create(UserDto userDto)
        {
            var user = _autoMapper.Map<User>(userDto);

            // validation
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new AppException("Adgangskode er påkrævet.");

            if (_userRepository.CheckIfEmailAddressTaken(user.EmailAddress))
                throw new AppException("E-mailaddressen \"" + user.EmailAddress + "\" er allerede registreret.");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //Create user as customer and manually assign admins in the database.
            user.Role = Enums.Role.Customer;

            _userRepository.Add(user);

            return _autoMapper.Map<UserDto>(user);
        }

        public async Task<Guid> GeneratePasswordRecoveryKeyAsync(string emailAddress)
        {
            var user = _userRepository.GetByEmailAddress(emailAddress);
            if (user == null)
            {
                throw new ArgumentException("Der er ikke en bruger med den angivne e-mailadresse.");
            };
            var recoveryKey = Guid.NewGuid();
            user.PasswordRecoveryKey = recoveryKey;
            _userRepository.Update(user);
            await SendRecoveryEmailAsync(emailAddress, recoveryKey);
            return recoveryKey;
        }

        public void ChangePassword(string newPassword, string recoveryKey)
        {
            var user = _userRepository.GetByRecoveryKey(recoveryKey);
            if (user == null)
            {
                throw new ArgumentException("Der er ikke en bruger med den angivne kode til ændring af adgangskode.");
            };

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordRecoveryKey = null;

            _userRepository.Update(user);
        }

        public bool UserExists(string emailAddress)
        {
            var user = _userRepository.GetByEmailAddress(emailAddress);
            return user != null;
        }

        private async Task<Email> SendRecoveryEmailAsync(string emailAddress, Guid passwordRecoveryKey)
        {
            var email = new Email()
            {
                HtmlBody = await GetRecoveryEmailHtmlAsync(emailAddress, passwordRecoveryKey, _appSettings.RootUrl),
                TextBody = $"Klik på dette link for at ændre din adgangskode: {_appSettings.RootUrl}/password-change/{passwordRecoveryKey.ToString()}",
                Subject = $"Shelfer - Link til ændring af adgangskode",
                Receiver = emailAddress
            };
            return _emailService.EnqueueEmail(email);
        }

        private async Task<string> GetRecoveryEmailHtmlAsync(string emailAddress, Guid passwordRecoveryKey, string rootUrl)
        {
            var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), $"Views\\Emails\\PasswordChange.cshtml");
            string razorTemplate = System.IO.File.ReadAllText(templatePath);
            var recoveryLink = _appSettings.RootUrl + "/password-change/" + passwordRecoveryKey;
            var passwordChangeEmailViewModel = new PasswordChangeEmailViewModel() { RecoveryLink = recoveryLink };
            var model = new System.Dynamic.ExpandoObject();
            var modelDict = (IDictionary<string, object>)model;
            modelDict["RecoveryLink"] = recoveryLink;
            return await _razorEngine.CompileRenderAsync("passwordChange", razorTemplate, model);
        }

        public void Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}