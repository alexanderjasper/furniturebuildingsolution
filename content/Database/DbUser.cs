using System;
using System.ComponentModel.DataAnnotations.Schema;
using FurnitureBuildingSolution.Helpers;

namespace FurnitureBuildingSolution.Database
{
    [Table("Users")]
    public class DbUser : BaseDbEntity
    {
        public string EmailAddress { get; set; }
        public Enums.Role Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid? PasswordRecoveryKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}