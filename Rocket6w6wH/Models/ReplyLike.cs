using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class ReplyLike
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "回覆編號")]
        public int ReplyId { get; set; }

        [JsonIgnore]
        [ForeignKey("ReplyId")]
        public virtual Reply Replys { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "按讚人ID")]
        public int LikeUserId { get; set; }
        public virtual Member Member { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}