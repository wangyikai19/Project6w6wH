namespace Rocket6w6wH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stores
    {
        [Key] //PrimaryKey
        [Display(Name = "�s��")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //�۰ʲ���
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "���a�W��")]
        public string StoreName { get; set; }

        [Display(Name = "���a����a�}")]
        public string AddressCh { get; set; }

        [Display(Name = "���a�^��a�}")]
        public string AddressEn { get; set; }
        [Display(Name = "���O�_�I")]
        public int? PriceStart { get; set; }
        [Display(Name = "���O���I")]
        public int? PriceEnd { get; set; }

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "�q��")]
        public string Phone { get; set; }

        [Display(Name = "�w�����}")]
        public string ReserveUrl { get; set; }

        [Display(Name = "���a���}")]
        public string StoreUrl { get; set; }

        [Display(Name = "���aGoogleID")]
        public string StoreGoogleId { get; set; }

        [Display(Name = "��~�ɶ�")]
        public string BusinessHours { get; set; }

        [Display(Name = "���aX�y��")]
        public decimal? XLocation { get; set; }

        [Display(Name = "���aY�y��")]
        public decimal? YLocation { get; set; }

        [Display(Name = "���a����")]
        public string StoreTags { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public virtual ICollection<StoreComments> StoreComments { get; set; }
        public virtual ICollection<StorePictures> StorePictures { get; set; }


    }
}
