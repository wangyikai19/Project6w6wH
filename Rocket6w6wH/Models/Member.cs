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
        [Display(Name = "�s��")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0}����")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0}����")]
        [MaxLength(50)]
        [Display(Name = "�m�W")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}����")]
        [MaxLength(50)]
        [Display(Name = "��a")]
        public string Country { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "�ͤ�")]
        public DateTime? Birthday { get; set; }
        [Display(Name = "�Ӥ�")]
        public string Photo { get; set; }
        [Display(Name = "����")]
        public string Badge { get; set; }
        [Display(Name = "�j�Y��")]
        public string MemberPictureUrl { get; set; }

        [Display(Name = "�ʧO")]
        public int Gender { get; set; }
        // �ɯ��ݩ� - �@��h���Y

        public virtual ICollection<StoreComments> StoreComments { get; set; }//�ϦV�ɯ��ݩ�
        public virtual ICollection<Reply> Reply { get; set; }//�ϦV�ɯ��ݩ�
        public virtual ICollection<ReplyLike> ReplyLike { get; set; }//�ϦV�ɯ��ݩ�
        public virtual ICollection<CommentLike> CommentLike { get; set; }//�ϦV�ɯ��ݩ�
    }
}
