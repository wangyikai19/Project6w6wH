using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Rocket6w6wH.Models
{
    public class CommnetReport
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "回覆編號")]
        public int CommentId { get; set; }
        [JsonIgnore]
        [ForeignKey("CommentId")]
        public virtual StoreComments StoreComments { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "檢舉人ID")]
        public int ReportUserId { get; set; }
        public virtual Member Member { get; set; }
        [MaxLength(50)]
        [Display(Name = "檢舉原因內容")]
        public string Comment { get; set; }
        public string Type { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}