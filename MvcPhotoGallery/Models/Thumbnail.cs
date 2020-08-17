using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPhotoGallery.Models
{
    public class Thumbnail
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [StringLength(255)]
        [Required]
        public string Cdn_path { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
    }
}
