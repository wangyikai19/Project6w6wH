using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class StoreComments
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        
        public int MemberId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int Stars { get; set; }

        // 導航屬性
        public virtual Member Member { get; set; }
    }
}