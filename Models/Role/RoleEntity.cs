using System.ComponentModel.DataAnnotations;

namespace CRUDproject.Models.RoleDto
{
    public class RoleEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }=string .Empty;


    }
}
