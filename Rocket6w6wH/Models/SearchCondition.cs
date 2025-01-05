using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class SearchCondition
    {
        [Key] //PrimaryKey
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自動產生
        public int Id { get; set; }
        [Display(Name = "分類")]
        public string Group { get; set; }
        [Display(Name = "值")]
        public string PVal { get; set; }
        [Display(Name = "名稱")]
        public string MVal { get; set; }
        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}