namespace Rocket6w6wH.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stores
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }

        public string AddressCh { get; set; }

        public string AddressEn { get; set; }

        public int? PriceStart { get; set; }

        public int? PriceEnd { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public string ReserveUrl { get; set; }
        public string StoreUrl { get; set; }
        public string StoreGoogleId { get; set; }

        public string BusinessHours { get; set; }

        public decimal? XLocation { get; set; }

        public decimal? YLocation { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
        public string StoreTags { get; set; }
    }
}
