using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EcommerceApp.API.Models
{
    public partial class EcommerceDBContext : DbContext
    {
        public EcommerceDBContext()
        {
        }

        public EcommerceDBContext(DbContextOptions<EcommerceDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<OrderStatusChange> OrderStatusChange { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=PF19DSEA\\SQLEXPRESS;Initial Catalog=EcommerceDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.MaterialCode)
                    .HasName("PK_MaterialName");

                entity.ToTable("Material", "dbo");

                entity.Property(e => e.MaterialCode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.MaterialName)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", "dbo");

                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.CustomerOrderId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DestinationAddress).HasMaxLength(250);

                entity.Property(e => e.MaterialCode)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.QuantityUnit).HasMaxLength(50);

                entity.Property(e => e.SenderAddress).HasMaxLength(250);

                entity.Property(e => e.WeightUnit).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("OrderStatus", "dbo");

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderStatusChange>(entity =>
            {
                entity.ToTable("OrderStatusChange", "dbo");

                entity.Property(e => e.ChangeDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
