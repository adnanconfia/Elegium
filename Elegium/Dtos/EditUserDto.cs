using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class EditUserDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public int? UserProfileId { get; set; }
        public bool Active { get; set; }
        public bool Banned { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Company { get; set; }
        public int IndustryId { get; set; }
        public bool ChangePassword { get; set; }
    }

    public class DeleteUserDto
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
