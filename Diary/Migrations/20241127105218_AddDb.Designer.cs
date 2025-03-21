﻿// <auto-generated />
using System;
using Diary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Diary.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241127105218_AddDb")]
    partial class AddDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Diary.Models.DiaryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("DiaryEntries");
                });

            modelBuilder.Entity("Diary.Models.RecordEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("DietDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Digesting")
                        .HasColumnType("int");

                    b.Property<bool>("IsAlcohol")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSexActivity")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSportActivity")
                        .HasColumnType("bit");

                    b.Property<string>("MedicationDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Menstruation")
                        .HasColumnType("int");

                    b.Property<string>("MentalDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MentalState")
                        .HasColumnType("int");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhysicalDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PhysicalState")
                        .HasColumnType("int");

                    b.Property<int>("SkinState1")
                        .HasColumnType("int");

                    b.Property<int>("SkinState2")
                        .HasColumnType("int");

                    b.Property<int>("SkinState3")
                        .HasColumnType("int");

                    b.Property<int>("SkinState4")
                        .HasColumnType("int");

                    b.Property<int>("SkinState5")
                        .HasColumnType("int");

                    b.Property<string>("SkinStateDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportActivityDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RecordEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
