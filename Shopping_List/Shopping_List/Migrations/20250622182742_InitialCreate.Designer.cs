using System;
using DAL.ContextDir;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Shopping_List.Migrations
{
    [DbContext(typeof(ShoppingListContext))]
    [Migration("20250622182742_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DBEntities.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasDefaultValue("SYSTEM");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("IconName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "SYSTEM",
                            Description = "מוצרים לניקוי הבית",
                            IconName = "cleaning",
                            IsActive = true,
                            Name = "מוצרי ניקיון",
                            SortOrder = 1
                        },
                        new
                        {
                            CategoryId = 2,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "SYSTEM",
                            Description = "גבינות וחלב",
                            IconName = "cheese",
                            IsActive = true,
                            Name = "גבינות",
                            SortOrder = 2
                        },
                        new
                        {
                            CategoryId = 3,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "SYSTEM",
                            Description = "ירקות ופירות טריים",
                            IconName = "fruits",
                            IsActive = true,
                            Name = "ירקות ופירות",
                            SortOrder = 3
                        },
                        new
                        {
                            CategoryId = 4,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "SYSTEM",
                            Description = "מוצרי בשר ודגים",
                            IconName = "meat",
                            IsActive = true,
                            Name = "בשר ודגים",
                            SortOrder = 4
                        },
                        new
                        {
                            CategoryId = 5,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedBy = "SYSTEM",
                            Description = "לחם ומוצרי מאפייה",
                            IconName = "bread",
                            IsActive = true,
                            Name = "מאפים",
                            SortOrder = 5
                        });
                });

            modelBuilder.Entity("DBEntities.Models.CompletedOrder", b =>
                {
                    b.Property<int>("CompletedOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompletedOrderId"));

                    b.Property<DateTime>("CompletedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TotalCategories")
                        .HasColumnType("int");

                    b.Property<int>("TotalItems")
                        .HasColumnType("int");

                    b.HasKey("CompletedOrderId");

                    b.HasIndex("CompletedAt")
                        .HasDatabaseName("IX_CompletedOrders_CompletedAt");

                    b.HasIndex("OrderNumber")
                        .IsUnique()
                        .HasDatabaseName("IX_CompletedOrders_OrderNumber_Unique");

                    b.HasIndex("SessionId");

                    b.ToTable("CompletedOrders");
                });

            modelBuilder.Entity("DBEntities.Models.CompletedOrderItem", b =>
                {
                    b.Property<int>("CompletedOrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompletedOrderItemId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValue("Normal");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("יחידה");

                    b.HasKey("CompletedOrderItemId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OrderId");

                    b.ToTable("CompletedOrderItems");
                });

            modelBuilder.Entity("DBEntities.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ShoppingCartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartItemId"));

                    b.Property<DateTime>("AddedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CheckedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsChecked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Notes")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValue("Normal");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("יחידה");

                    b.HasKey("ShoppingCartItemId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SessionId", "ProductName", "CategoryId")
                        .HasDatabaseName("IX_ShoppingCartItems_Session_Product_Category");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("DBEntities.Models.ShoppingSession", b =>
                {
                    b.Property<Guid>("ShoppingSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("SessionName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Active");

                    b.Property<int>("TotalItems")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("ShoppingSessionId");

                    b.ToTable("ShoppingSessions");
                });

            modelBuilder.Entity("DBEntities.Models.CompletedOrder", b =>
                {
                    b.HasOne("DBEntities.Models.ShoppingSession", "Session")
                        .WithMany("CompletedOrders")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("DBEntities.Models.CompletedOrderItem", b =>
                {
                    b.HasOne("DBEntities.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DBEntities.Models.CompletedOrder", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("DBEntities.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("DBEntities.Models.Category", "Category")
                        .WithMany("CartItems")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DBEntities.Models.ShoppingSession", "Session")
                        .WithMany("CartItems")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("DBEntities.Models.Category", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("DBEntities.Models.CompletedOrder", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("DBEntities.Models.ShoppingSession", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("CompletedOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
