

using DBEntities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DAL.ContextDir
{
    public class ShoppingListContext : DbContext
    {
        public ShoppingListContext(DbContextOptions<ShoppingListContext> options) : base(options)
        {
        }

        
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingSession> ShoppingSessions { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<CompletedOrder> CompletedOrders { get; set; }
        public DbSet<CompletedOrderItem> CompletedOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.IconName).HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(100).HasDefaultValue("SYSTEM");
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<ShoppingSession>(entity =>
            {
                entity.HasKey(e => e.ShoppingSessionId); 
                entity.Property(e => e.SessionName).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Active");
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.TotalItems).HasDefaultValue(0);
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasKey(e => e.ShoppingCartItemId); 
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Unit).HasMaxLength(20).HasDefaultValue("יחידה");
                entity.Property(e => e.Priority).HasMaxLength(10).HasDefaultValue("Normal");
                entity.Property(e => e.Notes).HasMaxLength(255);
                entity.Property(e => e.Quantity).HasDefaultValue(1);
                entity.Property(e => e.IsChecked).HasDefaultValue(false);
                entity.Property(e => e.AddedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Session)
                      .WithMany(s => s.CartItems)
                      .HasForeignKey(e => e.SessionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.SessionId, e.ProductName, e.CategoryId })
                      .HasDatabaseName("IX_ShoppingCartItems_Session_Product_Category");
            });

            modelBuilder.Entity<CompletedOrder>(entity =>
            {
                entity.HasKey(e => e.CompletedOrderId); 
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.CompletedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(e => e.Session)
                      .WithMany(s => s.CompletedOrders)
                      .HasForeignKey(e => e.SessionId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OrderNumber).IsUnique()
                      .HasDatabaseName("IX_CompletedOrders_OrderNumber_Unique");
                entity.HasIndex(e => e.CompletedAt)
                      .HasDatabaseName("IX_CompletedOrders_CompletedAt");
            });

            modelBuilder.Entity<CompletedOrderItem>(entity =>
            {
                entity.HasKey(e => e.CompletedOrderItemId); 
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Unit).HasMaxLength(20).HasDefaultValue("יחידה");
                entity.Property(e => e.Priority).HasMaxLength(10).HasDefaultValue("Normal");

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "מוצרי ניקיון",
                    Description = "מוצרים לניקוי הבית",
                    IconName = "cleaning",
                    SortOrder = 1,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = "SYSTEM"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "גבינות",
                    Description = "גבינות וחלב",
                    IconName = "cheese",
                    SortOrder = 2,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = "SYSTEM"
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "ירקות ופירות",
                    Description = "ירקות ופירות טריים",
                    IconName = "fruits",
                    SortOrder = 3,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = "SYSTEM"
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "בשר ודגים",
                    Description = "מוצרי בשר ודגים",
                    IconName = "meat",
                    SortOrder = 4,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = "SYSTEM"
                },
                new Category
                {
                    CategoryId = 5,
                    Name = "מאפים",
                    Description = "לחם ומוצרי מאפייה",
                    IconName = "bread",
                    SortOrder = 5,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = "SYSTEM"
                }
            );
        }
    }
}

