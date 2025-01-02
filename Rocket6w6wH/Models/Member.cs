namespace Rocket6w6wH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text.Json.Serialization;

    [Table("Member")]
    public partial class Member
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        public string Name { get; set; }
        public string MemberPictureUrl { get; set; }

        // �ɯ��ݩ� - �@��h���Y

        public virtual ICollection<StoreComments> StoreComments { get; set; }
    }
}
