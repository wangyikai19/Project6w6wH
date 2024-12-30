using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Area { get; set; }
        public string CountyName { get; set; }
        public string County{ get; set; }
    }
}