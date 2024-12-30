using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class StorePictures
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}