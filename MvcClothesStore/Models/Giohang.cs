using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MvcClothesStore.Models
{
    public class Giohang
    {
        dbQLClothesDataContext db = new dbQLClothesDataContext();
        public int iMaSP { set; get; }
        public string iTenSP { set; get; }
        public string iAnhSP { set; get; }
        public float iGiaHienTai { set; get; }
        public int iSL { set; get; }
        public string iSize { get; set; }
        public float iThanhTien
        {
            get { return iGiaHienTai * 1; }
        }
    }
}