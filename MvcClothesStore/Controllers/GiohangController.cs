using MvcClothesStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using FormCollection = System.Web.Mvc.FormCollection;

namespace MvcClothesStore.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        dbQLClothesDataContext db = new dbQLClothesDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> giohangs = Session["giohang"] as List<Giohang>;
            if (giohangs == null)
            {
                giohangs = new List<Giohang>();
                Session["giohang"] = giohangs;
            }
            return giohangs;
        }
        public ActionResult Index()
        {
            List<Giohang> giohangs = Session["giohang"] as List<Giohang>;
            return View(giohangs);
        }
        public RedirectToRouteResult XoaMatHang(int iMaSP)
        {
            dbQLClothesDataContext db = new dbQLClothesDataContext();
            var products = from table in db.SanPhams
                         where table.MaSP == iMaSP
                         select table;
            foreach (var prd in products)
            {
                db.SanPhams.DeleteOnSubmit(prd);
            }
            db.SubmitChanges();
            return RedirectToAction("Home", "ClothesStore", new { id = iMaSP });
        }
        public RedirectToRouteResult ThemVaoGio(int iMaSP,string iSize)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<Giohang>();
            }
            List<Giohang> giohang = Session["giohang"] as List<Giohang>;

            string size = "M";
            if (giohang.FirstOrDefault(m => m.iMaSP == iMaSP) == null)
            {
                SanPham sp = db.SanPhams.FirstOrDefault(n => n.MaSP == iMaSP);
                Giohang newItem = new Giohang()
                {
                    iMaSP = iMaSP,
                    iTenSP = sp.TenSP,
                    iSL = 1,
                    iAnhSP = sp.AnhSP,
                    iGiaHienTai = (float)sp.GiaHienTai,
                    iSize = size,

                };          
                giohang.Add(newItem);              
            }
            else
            {
                Giohang giohang1 = giohang.FirstOrDefault(m => m.iMaSP == iMaSP);
                giohang1.iSL++;
            }
            Session["tongtien"] = TongTien();
            return RedirectToAction("Index", "Giohang", new { id = iMaSP,iSize= size });
        }
        public RedirectToRouteResult ThemVaoGio1(int iMaSP, string iSize)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<Giohang>();
            }
            List<Giohang> giohang = Session["giohang"] as List<Giohang>;

            string size = "M";
            if (giohang.FirstOrDefault(m => m.iMaSP == iMaSP) == null)
            {
                SanPham sp = db.SanPhams.FirstOrDefault(n => n.MaSP == iMaSP);
                Giohang newItem = new Giohang()
                {
                    iMaSP = iMaSP,
                    iTenSP = sp.TenSP,
                    iSL = 1,
                    iAnhSP = sp.AnhSP,
                    iGiaHienTai = (float)sp.GiaHienTai,
                    iSize = size,

                };
                giohang.Add(newItem);
            }
            else
            {
                Giohang giohang1 = giohang.FirstOrDefault(m => m.iMaSP == iMaSP);
                giohang1.iSL++;
            }
            Session["tongtien"] = TongTien();
            DialogResult dlr = MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return RedirectToAction("Details", "ClothesStore", new { id = iMaSP, iSize = size });
        }
        public RedirectToRouteResult XoaKhoiGio(int iMaSP)
        {
            List<Giohang> giohang = Session["giohang"] as List<Giohang>;
            Giohang itemXoa = giohang.FirstOrDefault(m => m.iMaSP == iMaSP);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }
        public int a()
        {
            Random r = new Random();
            int n = r.Next(15, 50);
            return n;
        }
        public float TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> giohangs = Laygiohang();
            if (giohangs != null)
            {
                iTongSoLuong = giohangs.Sum(n => n.iSL);
            }
            return iTongSoLuong;
        }

        public float TongTien()
        {
            float iTongTien = 0;
            string ab = "";
            List<Giohang> giohangs = Laygiohang();
            if (giohangs != null)
            {
                iTongTien = giohangs.Sum(n => n.iThanhTien);
                ab = iTongTien.ToString();
            }
            return iTongTien;
        }
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                DialogResult dialogResult = MessageBox.Show("Bạn đã có tài khoản chưa?", "Thông báo", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    return RedirectToAction("SignIn", "User");
                }
                else if (dialogResult == DialogResult.No)
                {
                    return RedirectToAction("SignUp", "User");
                }
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Home", "ClothesStore");
            }
            List<Giohang> giohangs = Laygiohang();
            return View(giohangs);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            var id_max = db.Chitietdonhangs.Where(p => p.Id_DH > 0).Max(p => p.Id_DH);
            var madonhanh = id_max + 1;
            DonHang donHang = new DonHang();
            NguoiDung nguoiDung = (NguoiDung)Session["Taikhoan"];
            List<Giohang> giohangs = Laygiohang();
            donHang.Id_DH = a();
            donHang.TenKH = nguoiDung.HoVaTen;
            donHang.DiaChi = nguoiDung.diachi;
            donHang.Email = nguoiDung.email;
            donHang.Thongtin = "";
            donHang.TrangThai = false;
            donHang.Id_ND = nguoiDung.Id_ND;
            donHang.TongTien = TongTien();
            //Session["giohang"] = 
            db.DonHangs.InsertOnSubmit(donHang);
            db.SubmitChanges();
            foreach(var item in giohangs)
            {
                Chitietdonhang ctdh = new Chitietdonhang();
                ctdh.Id_DH = madonhanh;
                ctdh.MaSP = (int)item.iMaSP;
                ctdh.Soluong = item.iSL;
                ctdh.Dongia = (decimal)item.iGiaHienTai;
                db.Chitietdonhangs.InsertOnSubmit(ctdh);

            }
            db.SubmitChanges();
            Session["giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}