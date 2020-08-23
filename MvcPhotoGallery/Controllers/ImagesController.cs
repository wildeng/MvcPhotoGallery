using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPhotoGallery.Data;
using MvcPhotoGallery.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MvcPhotoGallery.Controllers
{
    public class ImagesController : Controller
    {
        private readonly PhotoGalleryContext _context;
        private IWebHostEnvironment Environment;

        public ImagesController(PhotoGalleryContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create(int? PhotogalleryId)
        {
            var image = new Models.Image();
            image.PhotoGalleryId = (int)PhotogalleryId;
            image.CreationDate = DateTime.Now;
            return PartialView("_UploadImage", image);
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PhotoGalleryId,Title,Cdn_path,CreationDate")] Models.Image image)
        {
            image.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

    // POST: Images/UploadImage
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> UploadImage(Models.Image image, IFormFileCollection files)
    {
    
      if (files.Count() == 0 || files.FirstOrDefault() == null)
      {
        ModelState.AddModelError(string.Empty, "You need  to upload file");
        return PartialView("_UploadImage", image);
      }

      var model = new Models.Image();
      foreach(var file in files)
      {
        if (file.Length == 0) { continue; }

        model.Title = image.Title;
        model.PhotoGalleryId = image.PhotoGalleryId;
        var fileName = Guid.NewGuid().ToString();
        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

        using (var img = SixLabors.ImageSharp.Image.Load(file.OpenReadStream()))
        {
          string name = String.Format("{0}{1}", fileName, extension).ToString();
          model.ThumbnailPath = Path.Combine("/GalleryImages/Thumbs/", name);
          model.ImagePath = Path.Combine("/GalleryImages/", name);

          System.Drawing.Size imageSize = new System.Drawing.Size(img.Width, img.Height);
          System.Drawing.Size thumbSize = NewSize(imageSize, new System.Drawing.Size(100,100));

         
          if(!ModelState.IsValid)
            return PartialView("_UploadImage", model);
          _context.Add(model);
          await _context.SaveChangesAsync();
          string wwwpath = this.Environment.WebRootPath.ToString();
          var thumbPath = String.Concat(wwwpath, model.ThumbnailPath.ToString());
          var imagePath = String.Concat(wwwpath, model.ImagePath.ToString());
          img.Save(imagePath);
          img.Mutate(x => x.Resize(thumbSize.Width, thumbSize.Height));
          img.Save(thumbPath);
         
        }
        
      }
      return PartialView("_UploadImage", model);
    }

    // POST: Images/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PhotoGalleryId,Title,Cdn_path,CreationDate")] Models.Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }

        private System.Drawing.Size NewSize(System.Drawing.Size imageSize, System.Drawing.Size newSize)
        {
      System.Drawing.Size finalSize;
          double tempval;

          if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
          {
            if (imageSize.Height > imageSize.Width)
            {
              tempval = newSize.Height / (imageSize.Height * 1.0);
            }
            else
            {
              tempval = newSize.Width / (imageSize.Width * 1.0);
            }
            finalSize = new System.Drawing.Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
          }
          else
          {
            finalSize = imageSize;
          }
          return finalSize;
        }    
    }
}
