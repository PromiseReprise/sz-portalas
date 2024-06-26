﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StaloŽaidimųPortalas.Data;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations
{
    [DbContext(typeof(AplikacijosDbContext))]
    [Migration("20240418144505_initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.StaloŽaidimas", b =>
                {
                    b.Property<int>("ŽaidimoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ŽaidimoId"));

                    b.Property<string>("Kategorija")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pavadinimas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ŽaidėjųKiekisMax")
                        .HasColumnType("int");

                    b.Property<int?>("ŽaidėjųKiekisMin")
                        .HasColumnType("int");

                    b.HasKey("ŽaidimoId");

                    b.ToTable("StaloŽaidimai");
                });
#pragma warning restore 612, 618
        }
    }
}
