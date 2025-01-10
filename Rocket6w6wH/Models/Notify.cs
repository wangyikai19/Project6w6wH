using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Rocket6w6wH.Models
{
    public class Notify
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "留言ID")]
        public int ReplyId { get; set; }

        [JsonIgnore]
        [ForeignKey("ReplyId")]
        public virtual Reply Reply { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "留言人ID")]
        public int ReplyUserId { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "評論ID")]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "評論人ID")]
        public int CommentUserId { get; set; }
        public int Check { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}