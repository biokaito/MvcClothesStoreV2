using MvcClothesStore.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;

namespace MvcClothesStore.Controllers
{
    public class AddProductController : Controller
    {
        // GET: AddProduct
        dbQLClothesDataContext db = new dbQLClothesDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Product(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoisp()
        {
            ViewBag.id_pl = new SelectList(db.PhanLoais.ToList().OrderBy(n => n.TenIdPl), "id_pl", "TenIdPl");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisp(SanPham sp, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload2)
        {
            ViewBag.id_pl = new SelectList(db.PhanLoais.ToList().OrderBy(n => n.TenIdPl), "id_pl", "TenIdPl");
            if(fileUpload == null || fileUpload2 == null)
            {
                ViewBag.Thongbao = "Please choose your image!";
                return View();
            }
                else 
                {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileUpload.FileName);
                    var filename2 = Path.GetFileName(fileUpload2.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    var path2 = Path.Combine(Server.MapPath("~/images"), filename2);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        fileUpload.SaveAs(path2);
                    }
                    sp.AnhSP = filename;
                    sp.Anh2SP = filename2;
                    db.SanPhams.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("Product");
            }
            
        }
        public ActionResult Chitietsp(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost,ActionName("Xoasp")]
        public ActionResult Xacnhanxoa(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Product");
        }
        [HttpGet]
        public ActionResult Suasp(int id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.id_pl = new SelectList(db.PhanLoais.ToList().OrderBy(n => n.TenIdPl), "id_pl", "TenIdPl",sp.MaSP);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasp(SanPham sp, HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload2)
        {
            ViewBag.id_pl = new SelectList(db.PhanLoais.ToList().OrderBy(n => n.TenIdPl), "id_pl", "TenIdPl");
            if(fileUpload == null || fileUpload2 == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileUpload.FileName);
                    var filename2 = Path.GetFileName(fileUpload2.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    var path2 = Path.Combine(Server.MapPath("~/images"), filename2);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Ảnh đã tồn tại!";

                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                        fileUpload2.SaveAs(path2);
                    }
                    sp.AnhSP = filename;
                    sp.Anh2SP = filename2;
                    UpdateModel(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("Product");
            }
        }


        //[HttpGet]
        //public ActionResult AddProduct()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult AddProduct(SanPham a)
        //{

        //    dbQLClothesDataContext ab = new dbQLClothesDataContext();
        //    var id_max = ab.SanPhams.Where(p => p.MaSP > 0).Max(p => p.MaSP);
        //    var id_random = id_max + 1;
        //    DateTime ngay = DateTime.Now;
        //    a.NgayDang = ngay;
        //    ab.SanPhams.InsertOnSubmit(a);
        //    ab.SubmitChanges();
        //    return View("Home","ClothesStore");
        //}
    }
}
