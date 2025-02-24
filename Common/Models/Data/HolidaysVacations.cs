using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.Data
{
    [Table("Holidays_Vacations")] // Ensure table name matches the database
    public class HolidaysVacations
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string? Description { get; set; } // ? Nullable to match the database

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; } // ? Required to match your table

        [Required]
        [Column("type")]
        public string Type { get; set; } // ? Required to match the table

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } // ? Nullable to match the database

    }
}