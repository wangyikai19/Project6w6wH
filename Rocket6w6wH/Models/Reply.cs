using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class Reply
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "評論ID")]
        public int CommentId { get; set; }

        [JsonIgnore]
        [ForeignKey("CommentId")]
        public virtual StoreComments StoreComments { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "留言人ID")]
        public int ReplyUserId { get; set; }
        [JsonIgnore]
        [ForeignKey("ReplyUserId")]
        public virtual Member Member { get; set; }
        [Display(Name = "留言內容")]
        public string ReplyContent { get; set; }
        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
        public string ReplyOnlyCode { get; set; }
        public virtual ICollection<ReplyLike> ReplyLike { get; set; }
        public virtual ICollection<notify> notify { get; set; }
        public Reply()
        {
            ReplyLike = new HashSet<ReplyLike>();
        }
    }
}