using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.Json.Serialization;

namespace Rocket6w6wH.Models
{
    public class CommentPictures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CommentId { get; set; }
        [JsonIgnore]
        [ForeignKey("CommentId")]
        public virtual StoreComments StoreComments { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}