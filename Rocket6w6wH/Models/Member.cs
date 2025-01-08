namespace Rocket6w6wH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Newtonsoft.Json;

    public partial class Member
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "國家")]
        public string Country { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }
        [Display(Name = "照片")]
        public string Photo { get; set; }
        [Display(Name = "勳章")]
        public string Badge { get; set; }
        [Display(Name = "大頭照")]
        public string MemberPictureUrl { get; set; }

        [Display(Name = "性別")]
        public int Gender { get; set; }
        // 導航屬性 - 一對多關係

        public virtual ICollection<StoreComments> StoreComments { get; set; }//反向導航屬性
        public virtual ICollection<Reply> Reply { get; set; }//反向導航屬性
        public virtual ICollection<ReplyLike> ReplyLike { get; set; }//反向導航屬性
        public virtual ICollection<CommentLike> CommentLike { get; set; }//反向導航屬性
    }
}
