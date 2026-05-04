using System;
using System.ComponentModel.DataAnnotations;
namespace CRUDproject.Dtos.JobDto
{
    public class JobDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "oyy aa division baki chhe lakh ne ")]
        [StringLength(50,MinimumLength =3)]
        public string? Division { get; set; }
        [Required]
        public int? SchoolId { get; set; }
        [Required]
        public int? PhotographerId { get; set; }
        [Required]
        public DateTime? ShootDate { get; set; }
        [Required]
        public string? EventType { get; set; }
        [Required]
        public string? ShootCategory { get; set; }
        [Required]
        public string? SaleType { get; set; }
        [Required]
        public int? ShootId { get; set; }
        [Required]
        public string? Status { get; set; }

        public int CreatedBy { get; set; }

        public int? UserId { get; set; }
    }
}
