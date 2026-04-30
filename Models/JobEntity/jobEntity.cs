using CRUDproject.Models.AuthUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDproject.Models
{
    
    public class JobEntity
    {
        
        public int Id { get; set; }

        
        public string? Division { get; set; }
        public int? SchoolId { get; set; }
        public int? PhotographerId { get; set; }
        public DateTime? ShootDate { get; set; }
        public string? EventType { get; set; }     
        public string? ShootCategory { get; set; }

        public string? SaleType { get; set; }
        public int? ShootId { get; set; }
        public string? Status { get; set; }
        public int? CreatedBy { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }


    }
}
