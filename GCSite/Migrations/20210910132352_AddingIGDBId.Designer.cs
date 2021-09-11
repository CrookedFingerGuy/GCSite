﻿// <auto-generated />
using System;
using GCSite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GCSite.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210910132352_AddingIGDBId")]
    partial class AddingIGDBId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GCSite.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BoxSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoxStyle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverArtPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("CurrentValueCib")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CurrentValueMo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Developer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("GamePlayLength")
                        .HasColumnType("bigint");

                    b.Property<long?>("GameSizeMb")
                        .HasColumnType("bigint");

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IGDBId")
                        .HasColumnType("int");

                    b.Property<string>("ManualScanPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MediaCount")
                        .HasColumnType("int");

                    b.Property<string>("MediaScanPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MinSpecs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Platform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReleaseDateEu")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ReleaseDateJp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ReleaseDateUs")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("RetailPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ThreeDBoxmodelPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThumbnailPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GCSite.Models.GameSearchViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Developer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Platform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Publisher")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReleaseDateUs")
                        .HasColumnType("datetime2");

                    b.Property<string>("ThumbnailPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameSearchViewModel");
                });
#pragma warning restore 612, 618
        }
    }
}
