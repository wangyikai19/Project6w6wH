using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class StoreComments
    {
        [Key] //PrimaryKey
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自動產生
        public int Id { get; set; }

        [Display(Name = "店家ID")]
        [Required(ErrorMessage = "{0}必填")]
        public int StoreId { get; set; }

        [JsonIgnore]
        [ForeignKey("StoreId")]
        public virtual Stores Stores { get; set; }

        [Display(Name = "評論人ID")]
        [Required(ErrorMessage = "{0}必填")]
        public int MemberId { get; set; }

        [JsonIgnore]
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
        [MaxLength(50)]
        [Display(Name = "評論內容")]
        public string Comment { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        [Display(Name = "星數")]
        public int Stars { get; set; }
        [Display(Name = "標籤")]
        public string Label { get; set; }
        public virtual ICollection<CommentPictures> CommentPictures { get; set; }
        public virtual ICollection<Reply> Reply { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<CommnetReport> CommnetReports { get; set; }
    }
}