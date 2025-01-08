using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Rocket6w6wH.Models
{
	public class CollectStore
	{
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "店家ID")]
        public int StoreId { get; set; }

        [JsonIgnore]
        [ForeignKey("StoreId")]
        public virtual Stores Stores { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "按讚人ID")]
        public int MemberId { get; set; }
        [JsonIgnore]
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}