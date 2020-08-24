using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPhotoGallery.Data;
using MvcPhotoGallery.Models;

namespace MvcPhotoGallery.Controllers
{
  public class PhotoGalleriesController : Controller
  {
    private readonly PhotoGalleryContext _context;

    public PhotoGalleriesController(PhotoGalleryContext context)
    {
      _context = context;
    }

    // GET: PhotoGalleries
    public async Task<IActionResult> Index()
    {

      return View(await _context.PhotoGalleries.ToListAsync());
    }

    // GET: PhotoGalleries/Details/5
    public async Task<IActionResult> Details(int? id, int page = 1, int pageSize = 20)
    {
      if (id == null)
      {
        return NotFound();
      }

      ViewBag.galleryId = id;

      var photoGallery = await _context.PhotoGalleries
          .FirstOrDefaultAsync(m => m.Id == id);
      if (photoGallery == null)
      {
        return NotFound();
      }


      photoGallery.Images.Content = _context.Images
        .Where(x => x.PhotoGalleryId == id)
        .OrderByDescending(x => x.CreationDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

      photoGallery.Images.TotalRecords = _context.Images
        .Where(x => x.PhotoGalleryId == id).Count();

      photoGallery.Images.CurrentPage = page;
      photoGallery.Images.PageSize = pageSize;

      if (page > 1)
        photoGallery.Images.HasPreviousPage = true;

      if (page * pageSize < photoGallery.Images.TotalRecords)
        photoGallery.Images.HasNextPage = true;



      return View(photoGallery);
    }

    // GET: PhotoGalleries/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: PhotoGalleries/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title")] PhotoGallery photoGallery)
    {
      photoGallery.CreationDate = DateTime.Now;
      if (ModelState.IsValid)
      {
        _context.Add(photoGallery);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(photoGallery);
    }

    // GET: PhotoGalleries/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var photoGallery = await _context.PhotoGalleries.FindAsync(id);
      if (photoGallery == null)
      {
        return NotFound();
      }
      ViewBag.galleryId = photoGallery.Id;
      return View(photoGallery);
    }

    // POST: PhotoGalleries/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] PhotoGallery photoGallery)
    {
      if (id != photoGallery.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(photoGallery);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!PhotoGalleryExists(photoGallery.Id))
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
      return View(photoGallery);
    }

    // GET: PhotoGalleries/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var photoGallery = await _context.PhotoGalleries
          .FirstOrDefaultAsync(m => m.Id == id);
      if (photoGallery == null)
      {
        return NotFound();
      }

      return View(photoGallery);
    }

    // POST: PhotoGalleries/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var photoGallery = await _context.PhotoGalleries.FindAsync(id);
      _context.PhotoGalleries.Remove(photoGallery);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    // return image modal
    public IActionResult GetImageModal()
    {
      return PartialView("_UploadImage");
    }

    private bool PhotoGalleryExists(int id)
    {
      return _context.PhotoGalleries.Any(e => e.Id == id);
    }
  }
}
