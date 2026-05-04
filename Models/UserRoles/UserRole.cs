using CRUDproject.Models.AuthUser;
using CRUDproject.Models.RoleDto;
using System.Data;

namespace CRUDproject.Models.UserRoles
{
    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = null!;
    }
}
