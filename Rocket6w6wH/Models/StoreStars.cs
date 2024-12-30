using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class StoreStars
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int MemberId { get; set; }
        public int Stars { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}