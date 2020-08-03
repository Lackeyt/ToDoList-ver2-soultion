using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    private readonly ToDoListContext _db;
    public ItemsController(ToDoListContext db)
    {
      _db = db;
    }
    public ActionResult Details(int id)
    {
      var thisItem = _db.Items
          .Include(item => item.Categories)
          .ThenInclude(join => join.Category)
          .FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }
    public ActionResult Create()
    {
    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    return View();
    }
    public ActionResult Edit(int id)
    {
    var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item)
    {
        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

//     public ActionResult Delete(int id)
//     {
//       var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
//       return View(thisItem);
//     }

//     [HttpPost, ActionName("Delete")]
//     public ActionResult DeleteConfirmed(int id)
//     {
//       var thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
//       _db.Items.Remove(thisItem);
//       _db.SaveChanges();
//       return RedirectToAction("Index");
//     }

    [HttpPost]
    public ActionResult Create(Item item, int CategoryId)
    {
      _db.Items.Add(item);
    if (CategoryId != 0)
    {
        _db.CategoryItem.Add(new CategoryItem() { CategoryId = CategoryId, ItemId = item.ItemId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
    }
    public ActionResult Index()
    {
      List<Item> model = _db.Items.ToList();
      return View(model);
    }
  }
}

