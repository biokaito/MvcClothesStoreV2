using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcClothesStore.Models
{
    public class loai
    {
        public byte id_pl { get; set; }
        [Required]
        [StringLength(255)]
        public string TenldPI { get; set; }
    }
}