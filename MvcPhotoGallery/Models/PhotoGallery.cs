using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcPhotoGallery.Models
{
    public class PhotoGallery
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Image> Images { get; set; }

    }
}
