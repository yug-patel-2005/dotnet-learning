using System.Collections.Generic;
using CRUDproject.Dtos.RoleDto;

namespace CRUDproject.Dtos.UserRoleDto
{
    public class UserRoleResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<RoleDto.RoleDto> Roles { get; set; } = new List<RoleDto.RoleDto>();
    }
}
