using System;
using FurnitureBuildingSolution.Helpers;

namespace FurnitureBuildingSolution.Entities
{
    public class User : BaseEntity
    {
        public Enums.Role Role { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid? PasswordRecoveryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}