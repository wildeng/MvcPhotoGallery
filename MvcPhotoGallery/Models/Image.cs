using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcPhotoGallery.Models
{
    public class Image
    {
        public int Id { get; set; }

        [ForeignKey("PhotoGallery")]
        public int PhotoGalleryId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [StringLength(255)]
        [Required]
        public string ImagePath { get; set; }

        [StringLength(255)]
        [Required]
        public string ThumbnailPath { get; set; }

        [BindNever]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public Image()
        {
            CreationDate = DateTime.Now;
        }
    }
}
