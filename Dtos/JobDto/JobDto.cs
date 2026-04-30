using System;
using System.ComponentModel.DataAnnotations;
namespace CRUDproject.Dtos.JobDto
{
    public class JobDto
    {
        [Required (ErrorMessage = "oyy aa division baki chhe lakh ne ")]
        [StringLength(50,MinimumLength =3)]
        public string? Division { get; set; }
        public int? SchoolId { get; set; }
        public int? PhotographerId { get; set; }
        public DateTime? ShootDate { get; set; }
        public string? EventType { get; set; }
        public string? ShootCategory { get; set; }
        public string? SaleType { get; set; }
        public int? ShootId { get; set; }
        public string? Status { get; set; }

        public int CreatedBy { get; set; }

        public int UserId { get; set; }
    }
}
