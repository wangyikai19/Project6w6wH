using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Rocket6w6wH.Models
{
    public class Follow
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "被追蹤用戶編號")]
        public int FollowUserId { get; set; }
        [JsonIgnore]
        [ForeignKey("FollowUserId")]
        public virtual Member FollowUser { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "追蹤人用戶編號")]
        public int UserId { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}