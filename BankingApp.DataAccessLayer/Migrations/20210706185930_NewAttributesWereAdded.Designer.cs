﻿// <auto-generated />
using System;
using BankingApp.DataAccessLayer.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankingApp.DataAccessLayer.Migrations
{
    [DbContext(typeof(BankingDbContext))]
    [Migration("20210706185930_NewAttributesWereAdded")]
    partial class NewAttributesWereAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BankingApp.Entities.Entities.DepositeHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CalculationFormula")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CalulationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DepositeSum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MonthsCount")
                        .HasColumnType("int");

                    b.Property<int>("Percents")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DepositeHistories");
                });

            modelBuilder.Entity("BankingApp.Entities.Entities.DepositeHistoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepositeHistoryId")
                        .HasColumnType("int");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<int>("Percents")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalMonthSum")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("DepositeHistoryId");

                    b.ToTable("DepositeHistoryItems");
                });

            modelBuilder.Entity("BankingApp.Entities.Entities.DepositeHistoryItem", b =>
                {
                    b.HasOne("BankingApp.Entities.Entities.DepositeHistory", "DepositeHistory")
                        .WithMany("DepositeHistoryItems")
                        .HasForeignKey("DepositeHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DepositeHistory");
                });

            modelBuilder.Entity("BankingApp.Entities.Entities.DepositeHistory", b =>
                {
                    b.Navigation("DepositeHistoryItems");
                });
#pragma warning restore 612, 618
        }
    }
}
