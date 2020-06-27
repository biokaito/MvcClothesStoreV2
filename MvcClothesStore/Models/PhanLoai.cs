using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcClothesStore.Models
{
    public class Phanloais
    {
        public loai Loai { get; set; }
        [Required]
        public byte loai_id { get; set; }
    }
}