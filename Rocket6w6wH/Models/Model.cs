using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;

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
        public virtual DbSet<StoreStars> StoreStars { get; set; }
        public virtual DbSet<StoreComments> StoreComments { get; set; }
        public virtual DbSet<Configs> Configs { get; set; }
        public virtual DbSet<SearchCondition> SearchCondition { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<StorePictures> StorePictures { get; set; }
        public virtual DbSet<CommentPictures> CommentPictures { get; set; }
        public virtual DbSet<CommentMessage> CommentMessage { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stores>()
                .Property(e => e.XLocation)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Stores>()
                .Property(e => e.YLocation)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StoreComments>()
                .HasRequired(sc => sc.Member) // StoreComments 必須有 Member
                .WithMany(m => m.StoreComments) // Member 可以有多個 StoreComments
                .HasForeignKey(sc => sc.MemberId); // 外鍵是 MemberId

            modelBuilder.Entity<CommentMessage>()
               .HasRequired(m => m.Member)  // 表示 Member 是必須的
               .WithMany(mem => mem.CommentMessage) // Member 與 Messages 的一對多關係
               .HasForeignKey(m => m.MemberId) // 外鍵是 MemberId
               .WillCascadeOnDelete(false); // 禁用級聯刪除

            base.OnModelCreating(modelBuilder);
        }
    }
}
