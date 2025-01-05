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
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自動產生
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "店家名稱")]
        public string StoreName { get; set; }

        [Display(Name = "店家中文地址")]
        public string AddressCh { get; set; }

        [Display(Name = "店家英文地址")]
        public string AddressEn { get; set; }
        [Display(Name = "消費起點")]
        public int? PriceStart { get; set; }
        [Display(Name = "消費終點")]
        public int? PriceEnd { get; set; }

        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "電話")]
        public string Phone { get; set; }

        [Display(Name = "預約網址")]
        public string ReserveUrl { get; set; }

        [Display(Name = "店家網址")]
        public string StoreUrl { get; set; }

        [Display(Name = "店家GoogleID")]
        public string StoreGoogleId { get; set; }

        [Display(Name = "營業時間")]
        public string BusinessHours { get; set; }

        [Display(Name = "店家X座標")]
        public decimal? XLocation { get; set; }

        [Display(Name = "店家Y座標")]
        public decimal? YLocation { get; set; }

        [Display(Name = "店家標籤")]
        public string StoreTags { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        public virtual ICollection<StoreComments> StoreComments { get; set; }
        public virtual ICollection<StorePictures> StorePictures { get; set; }


    }
}
