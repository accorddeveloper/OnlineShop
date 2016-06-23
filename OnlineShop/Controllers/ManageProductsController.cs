using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class ManageProductsController : Controller
    {
        private OnlineShopDb db = new OnlineShopDb();

        // GET: ManageProducts
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: ManageProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ManageProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            // BEGIN: Tạo ra dropdownlist cho trường Status
            var statusItems = new[]
            {
                new { Id = "ACTIVE", Name = "Active" },
                new { Id = "DEACTIVE", Name = "Deactive" },
            };

            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường Status

            // BEGIN: Tạo ra dropdownlist cho trường DisplayPosition
            var displayPositionItems = new[]
            {
                new { Id = "DEFAULT", Name = "Default" },
                new { Id = "SLIDE", Name = "Slide" },
                new { Id = "TOP", Name = "Top Products" },
            };

            ViewBag.DisplayPosition = new SelectList(displayPositionItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường DisplayPosition

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,CategoryId,Code,Name,Price,Stock,Description,Notes,Discount,DisplayPosition,Status,SortOrder")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;
                product.ImageUrl = "#";
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: ManageProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);

            // BEGIN: Tạo ra dropdownlist cho trường Status
            var statusItems = new[]
            {
                new { Id = "ACTIVE", Name = "Active" },
                new { Id = "DEACTIVE", Name = "Deactive" },
            };

            ViewBag.Status = new SelectList(statusItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường Status

            // BEGIN: Tạo ra dropdownlist cho trường DisplayPosition
            var displayPositionItems = new[]
            {
                new { Id = "DEFAULT", Name = "Default" },
                new { Id = "SLIDE", Name = "Slide" },
                new { Id = "TOP", Name = "Top Products" },
            };

            ViewBag.DisplayPosition = new SelectList(displayPositionItems, "Id", "Name");
            // END: Tạo ra dropdownlist cho trường DisplayPosition

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,Code,Name,Price,Stock,Description,Notes,Discount,DisplayPosition,Status,SortOrder,ImageUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedDate = DateTime.Now;                
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: ManageProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ManageProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region UPLOAD DETAILS IMAGES
       
        // TUNG.NGO
        public ActionResult UploadDetailsImages(int id)
        {
            using (var db = new OnlineShopDb())
            {
                var productImages = db.ProductImages.Where(x => x.ProductId == id).ToList();
                return View(productImages);
            }            
        }

        // TUNG.NGO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDetailsImages(HttpPostedFileBase file, int id)
        {
            var defaultFolderToSaveFile = "~/Uploads/Images/Products/" + id + "/Details/";

            // BEGIN: Kiểm tra nếu chưa tồn tại thư mục trên thì tạo mới. 
            if (Directory.Exists(Server.MapPath(defaultFolderToSaveFile)) == false)
            {
                Directory.CreateDirectory(Server.MapPath(defaultFolderToSaveFile));
            }
            // END: Kiểm tra nếu chưa tồn tại thư mục trên thì tạo mới. 

            if (ModelState.IsValid)
            {
                // Kiểm tra nếu người dùng có chọn file
                if (file != null && file.ContentLength > 0)
                {
                    // Lấy tên file
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        // Đường dẫn đầy đủ trên Server gồm path + filename
                        var path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), fileName);

                        var i = 1;
                        while (System.IO.File.Exists(path))
                        {
                            path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), i + "_" + fileName); // id + ".jpg"
                            i++;
                        }

                        // Upload file lên Server ở thư mục ~/Uploads/...
                        file.SaveAs(path);

                        // Lấy imageurl để lưu vào database, có định dạng "~/Uploads/Products/Id/ten_file.jpg"
                        var imageUrl = defaultFolderToSaveFile + fileName;
                        if (i > 1)
                        {
                            imageUrl = defaultFolderToSaveFile + (i - 1) + "_" + fileName;
                        }

                        // Lưu thông tin image url vào table: ProductImages

                        var productImage = new ProductImage();
                        productImage.ProductId = id;
                        productImage.ImageUrl = imageUrl;

                        // INSERT
                        db.ProductImages.Add(productImage);
                        db.SaveChanges();

                        return RedirectToAction("UploadDetailsImages");
                    }
                }
            }
            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult DeleteProductImage(int deleteId)
        {
            using (var db = new OnlineShopDb())
            {
                var item = db.ProductImages.Find(deleteId);

                if (item != null)
                {
                    // XÓA HÌNH
                    var pathToDetele = Server.MapPath(item.ImageUrl);
                    if (System.IO.File.Exists(pathToDetele))
                    {
                        System.IO.File.Delete(pathToDetele);
                    }

                    // XÓA TRONG DATABASE
                    var productId = item.ProductId;
                    db.ProductImages.Remove(item);
                    db.SaveChanges();

                    return RedirectToAction("UploadDetailsImages", "ManageProducts", new { id = productId });
                }

                return HttpNotFound();
            }
        }

        #region UPLOAD IMAGE

        // TUNG.NGO
        public ActionResult UploadImage(int id)
        {
            return View();
        }

        // TUNG.NGO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(HttpPostedFileBase file, int id)
        {
            var defaultFolderToSaveFile = "~/Uploads/Images/Products/" + id + "/";

            // BEGIN: Kiểm tra nếu chưa tồn tại thư mục trên thì tạo mới. 
            if (Directory.Exists(Server.MapPath(defaultFolderToSaveFile)) == false)
            {
                Directory.CreateDirectory(Server.MapPath(defaultFolderToSaveFile));
            }
            // END: Kiểm tra nếu chưa tồn tại thư mục trên thì tạo mới. 

            if (ModelState.IsValid)
            {
                // Kiểm tra nếu người dùng có chọn file
                if (file != null && file.ContentLength > 0)
                {
                    // Lấy tên file
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        // Đường dẫn đầy đủ trên Server gồm path + filename
                        var path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), fileName);

                        var i = 1;
                        while (System.IO.File.Exists(path))
                        {
                            path = Path.Combine(Server.MapPath(defaultFolderToSaveFile), i + "_" + fileName); // id + ".jpg"
                            i++;
                        }

                        // Upload file lên Server ở thư mục ~/Uploads/...
                        file.SaveAs(path);

                        // Lấy imageurl để lưu vào database, có định dạng "~/Uploads/Products/Id/ten_file.jpg"
                        var imageUrl = defaultFolderToSaveFile + fileName;
                        if (i > 1)
                        {
                            imageUrl = defaultFolderToSaveFile + (i - 1) + "_" + fileName;
                        }
                        // Lưu thông tin image url vào product
                        var product = db.Products.Find(id);
                        product.ImageUrl = imageUrl;
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        #endregion

        #region SUMMERNOTE UPLOAD IMAGES

        [HttpPost]
        public ActionResult SummernoteUploadImage()
        {
            foreach (var fileKey in Request.Files.AllKeys)
            {
                var file = Request.Files[fileKey];
                try
                {
                    var fileName = Path.GetFileName(file?.FileName);
                    if (fileName != null)
                    {
                        var path = Server.MapPath("~/Uploads/SummernoteImages");
                        if (System.IO.File.Exists(path) == false)
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }

                        path = Path.Combine(Server.MapPath("~/Uploads/SummernoteImages"), fileName);
                        file.SaveAs(path);
                        return Json(new { Url = Url.Content("~/Uploads/SummernoteImages/" + fileName) });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Message = $"Error in saving file ({ex.Message})" });
                }
            }
            return Json(new { Message = "File saved" });
        }

        #endregion
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
