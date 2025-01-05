using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class City
    {
        [Key] //PrimaryKey
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自動產生
        public int Id { get; set; }
        [Display(Name = "地區")]
        public string Area { get; set; }
        [Display(Name = "縣市英文")]
        public string CountryName { get; set; }
        [Display(Name = "縣市中文")]
        public string Country{ get; set; }
    }
}