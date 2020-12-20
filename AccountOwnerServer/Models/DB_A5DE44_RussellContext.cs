using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpotOnAccountServer.Models
{
    public partial class DB_A5DE44_RussellContext : DbContext
    {
        public DB_A5DE44_RussellContext()
        {
        }

        public DB_A5DE44_RussellContext(DbContextOptions<DB_A5DE44_RussellContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CallLists> CallLists { get; set; }
        public virtual DbSet<Specialists> Specialists { get; set; }
        public virtual DbSet<Treatments> Treatments { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=SQL6010.site4now.net;Database=DB_A5DE44_Russell;User Id=DB_A5DE44_Russell_admin;Password=SpotOns415;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CallLists>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LoginName).HasMaxLength(50);
            });

            modelBuilder.Entity<Specialists>(entity =>
            {
                entity.Property(e => e.Photo).HasMaxLength(50);

                entity.Property(e => e.Specialization).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Specialists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Specialists_Users");
            });

            modelBuilder.Entity<Treatments>(entity =>
            {
                entity.Property(e => e.Application).HasColumnType("text");

                entity.Property(e => e.DateTreated).HasMaxLength(50);

                entity.Property(e => e.DocId).HasMaxLength(256);

                entity.Property(e => e.Symptom).HasColumnType("ntext");

                entity.Property(e => e.TreatmentFor).HasMaxLength(250);

                entity.Property(e => e.UserId).HasMaxLength(256);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.ConfirmationCode).HasMaxLength(50);

                entity.Property(e => e.DateRegistered).HasMaxLength(50);

                entity.Property(e => e.EmailAddress).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LocalGovt).HasMaxLength(50);

                entity.Property(e => e.LoginName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.StateOrigin).HasMaxLength(50);

                entity.Property(e => e.SubscriptionDate).HasMaxLength(50);

                entity.Property(e => e.SubscriptionExpires).HasMaxLength(50);
            });
        }
    }
}
