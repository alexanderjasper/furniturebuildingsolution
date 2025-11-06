using FurnitureBuildingSolution.Helpers;

namespace FurnitureBuildingSolution.Dtos
{
    public class UserDto : BaseDto
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Enums.Role? Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}