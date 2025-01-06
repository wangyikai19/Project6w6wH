using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.Json.Serialization;
using System.Web;

namespace Rocket6w6wH.Models
{
    public class StoreComments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StoreId { get; set; }
        [JsonIgnore]
        [ForeignKey("StoreId")]
        public virtual Stores Stores { get; set; }
        public int MemberId { get; set; }
        [JsonIgnore]
        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        public string Comment { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int Stars { get; set; }
        public string Label { get; set; }
        public virtual ICollection<CommentPictures> CommentPictures { get; set; }
        public virtual ICollection<CommentMessage> CommentMessage { get; set; }
    }
}