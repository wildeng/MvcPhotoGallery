using System;
using System.Collections.Generic;

namespace MvcPhotoGallery.Models
{
  public class PagedList<T>
  {
    public List<T> Content { get; set; }
    public Int32 CurrentPage { get; set; }
    public Int32 PageSize { get; set; }
    public int TotalRecords { get; set; }
    public Boolean HasPreviousPage { get; set; }
    public Boolean HasNextPage { get; set; }
  }
}
