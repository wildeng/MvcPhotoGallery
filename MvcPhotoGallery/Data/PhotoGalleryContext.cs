using System;
using Microsoft.EntityFrameworkCore;
using MvcPhotoGallery.Models;

namespace MvcPhotoGallery.Data
{
    public class PhotoGalleryContext : DbContext
     {
        public PhotoGalleryContext(DbContextOptions<PhotoGalleryContext> options)
            : base(options)
        {
        }

        public DbSet<PhotoGallery> PhotoGalleries { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
    }
}
