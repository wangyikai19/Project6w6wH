using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Rocket6w6wH.Models
{
    public class StorePictures
    {
        [Key] //PrimaryKey
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //自動產生
        public int Id { get; set; }

        [Display(Name = "店家ID")]
        [Required(ErrorMessage = "{0}必填")]
        public int StoreId { get; set; }
        [JsonIgnore]
        [ForeignKey("StoreId")] //標示 MyOrg 是一個外鍵導航屬性
        public virtual Stores MyStores { get; set; } //導航屬性，用來表示與 Org 類別的關聯

        [Display(Name = "照片Url")]
        public string PictureUrl { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}