using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Rocket6w6wH.Models
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=Models")
        {
        }

        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Stores> Stores { get; set; }
        public virtual DbSet<StoreComments> StoreComments { get; set; }
        public virtual DbSet<Configs> Configs { get; set; }
        public virtual DbSet<SearchCondition> SearchCondition { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<StorePictures> StorePictures { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }
        public virtual DbSet<ReplyLike> ReplyLike { get; set; }
        public virtual DbSet<CommentPictures> CommentPictures { get; set; }
        public virtual DbSet<CommentMessage> CommentMessage { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stores>()
                .Property(e => e.XLocation)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Stores>()
                .Property(e => e.YLocation)
                .HasPrecision(18, 8);

            modelBuilder.Entity<StoreComments>()
                .HasRequired(sc => sc.Member)
                .WithMany(m => m.StoreComments)
                .HasForeignKey(sc => sc.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CommentMessage>()
                          .HasRequired(m => m.Member)  // 表示 Member 是必須的
                          .WithMany(mem => mem.CommentMessage) // Member 與 Messages 的一對多關係
                          .HasForeignKey(m => m.MemberId) // 外鍵是 MemberId
                          .WillCascadeOnDelete(false); // 禁用級聯刪除

            base.OnModelCreating(modelBuilder);

        }
    }
}
