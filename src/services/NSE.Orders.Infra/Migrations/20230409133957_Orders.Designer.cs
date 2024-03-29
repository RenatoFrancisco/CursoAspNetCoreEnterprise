﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSE.Orders.Infra.Data;

#nullable disable

namespace NSE.Orders.Infra.Migrations
{
    [DbContext(typeof(OrdersContext))]
    [Migration("20230409133957_Orders")]
    partial class Orders
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence<int>("MySequence")
                .StartsAt(1000L);

            modelBuilder.Entity("NSE.Orders.Domain.Orders.ItemOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductImage")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<decimal>("UnitValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("ItemsOrder", (string)null);
                });

            modelBuilder.Entity("NSE.Orders.Domain.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR MySequence");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("StatusOrder")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("UsedVoucher")
                        .HasColumnType("bit");

                    b.Property<Guid?>("VoucherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("VoucherId");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("NSE.Orders.Domain.Voucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DiscountType")
                        .HasColumnType("int");

                    b.Property<decimal?>("DiscountValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Percent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Used")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UsedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Vouchers", (string)null);
                });

            modelBuilder.Entity("NSE.Orders.Domain.Orders.ItemOrder", b =>
                {
                    b.HasOne("NSE.Orders.Domain.Orders.Order", "Order")
                        .WithMany("ItemsOrder")
                        .HasForeignKey("OrderId")
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("NSE.Orders.Domain.Orders.Order", b =>
                {
                    b.HasOne("NSE.Orders.Domain.Voucher", "Voucher")
                        .WithMany()
                        .HasForeignKey("VoucherId");

                    b.OwnsOne("NSE.Orders.Domain.Orders.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("OrderId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("City");

                            b1.Property<string>("Complement")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Complement");

                            b1.Property<string>("Neighborhood")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Neighborhood");

                            b1.Property<string>("Number")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Number");

                            b1.Property<string>("State")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("State");

                            b1.Property<string>("Street")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Street");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("varchar(100)")
                                .HasColumnName("ZipCode");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Address");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("NSE.Orders.Domain.Orders.Order", b =>
                {
                    b.Navigation("ItemsOrder");
                });
#pragma warning restore 612, 618
        }
    }
}
