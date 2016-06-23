using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    
    public class ManageCategoriesController : Controller
    {
        private OnlineShopDb db = new OnlineShopDb();

        // GET: ManageCategories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: ManageCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: ManageCategories/Create
        public ActionResult Create()
        {
            // BEGIN: Tạo ra dropdownlist cho trường Status
            var statusItems = new[]
            {
                new { Id = "ACTIVE", Name = "Active" },
                new { Id = "DEACTIVE", Name = "Deactive" },
            };

            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường Status

            // BEGIN: Tạo ra dropdownlist cho trường ParentId
            var categories = db.Categories.Where(x => x.ParentId == 0).ToList();

            var root = new Category();
            root.Id = 0;
            root.Name = "";

            categories.Add(root);

            ViewBag.ParentId = new SelectList(categories, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường ParentId

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentId,Name,SortOrder,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // BEGIN: Tạo ra dropdownlist cho trường Status
            var statusItems = new[]
            {
                new { Id = "ACTIVE", Name = "Active" },
                new { Id = "DEACTIVE", Name = "Deactive" },
            };

            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường Status

            // BEGIN: Tạo ra dropdownlist cho trường ParentId
            var categories = db.Categories.Where(x => x.ParentId == 0).ToList();

            var root = new Category();
            root.Id = 0;
            root.Name = "";

            categories.Add(root);

            ViewBag.ParentId = new SelectList(categories, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường ParentId


            return View(category);
        }

        // GET: ManageCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            // BEGIN: Tạo ra dropdownlist cho trường Status
            var statusItems = new[]
            {
                new { Id = "ACTIVE", Name = "Active" },
                new { Id = "DEACTIVE", Name = "Deactive" },
            };

            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường Status

            // BEGIN: Tạo ra dropdownlist cho trường ParentId
            var categories = db.Categories.Where(x => x.ParentId == 0).ToList();

            var root = new Category();
            root.Id = 0;
            root.Name = "";

            categories.Add(root);

            ViewBag.ParentId = new SelectList(categories, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường ParentId

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentId,Name,SortOrder,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // BEGIN: Tạo ra dropdownlist cho trường Status
            var statusItems = new[]
            {
                new { Id = "ACTIVE", Name = "Active" },
                new { Id = "DEACTIVE", Name = "Deactive" },
            };

            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường Status

            // BEGIN: Tạo ra dropdownlist cho trường ParentId
            var categories = db.Categories.Where(x => x.ParentId == 0).ToList();

            var root = new Category();
            root.Id = 0;
            root.Name = "";

            categories.Add(root);

            ViewBag.ParentId = new SelectList(categories, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường ParentId

            return View(category);
        }

        // GET: ManageCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: ManageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
