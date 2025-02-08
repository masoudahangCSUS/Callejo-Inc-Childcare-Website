using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.Data
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; } // Stores Google Drive direct link

        public bool IsPublished { get; set; } // Determines if the image is live

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
