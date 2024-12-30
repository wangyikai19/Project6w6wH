using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class SearchCondition
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public string PVal { get; set; }
        public string Mavl { get; set; }
        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}