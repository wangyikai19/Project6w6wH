using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Rocket6w6wH.Models
{
	public class CommentLike
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
        [Display(Name = "按讚人ID")]
        public int LikeUserId { get; set; }
        public virtual Member Member { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}