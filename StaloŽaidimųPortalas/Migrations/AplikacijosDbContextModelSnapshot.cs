﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StaloŽaidimųPortalas.Data;

#nullable disable

namespace StaloŽaidimųPortalas.Migrations
{
    [DbContext(typeof(AplikacijosDbContext))]
    partial class AplikacijosDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.Bendruomenė", b =>
                {
                    b.Property<int>("BendruomenėsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BendruomenėsId"));

                    b.Property<string>("Aprašymas")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("NaudotojoId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Pavadinimas")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ŽaidimoId")
                        .HasColumnType("int");

                    b.HasKey("BendruomenėsId");

                    b.ToTable("Bendruomenė");
                });

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.BendruomenėsNarys", b =>
                {
                    b.Property<int>("BendruomenėsId")
                        .HasColumnType("int");

                    b.Property<string>("NaudotojoId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ArAdministratorius")
                        .HasColumnType("bit");

                    b.Property<bool>("ArAktyvus")
                        .HasColumnType("bit");

                    b.HasKey("BendruomenėsId", "NaudotojoId");

                    b.ToTable("BendruomenėsNarys");
                });

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.BendruomenėsĮrašas", b =>
                {
                    b.Property<int>("ĮrašoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ĮrašoId"));

                    b.Property<int>("BendruomenėsId")
                        .HasColumnType("int");

                    b.Property<string>("NaudotojoId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Įrašas")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("ĮrašoId");

                    b.ToTable("BendruomenėsĮrašas");
                });

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.Skelbimas", b =>
                {
                    b.Property<int>("SkelbimoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkelbimoId"));

                    b.Property<string>("Aprašymas")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("NaudotojoId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Pavadinimas")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("ŽaidimoId")
                        .HasColumnType("int");

                    b.HasKey("SkelbimoId");

                    b.ToTable("Skelbimai");
                });

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.SkelbimoNarys", b =>
                {
                    b.Property<int>("SkelbimoId")
                        .HasColumnType("int");

                    b.Property<string>("NaudotojoId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SkelbimoId", "NaudotojoId");

                    b.ToTable("SkelbimųNariai");
                });

            modelBuilder.Entity("StaloŽaidimųPortalas.Models.Entities.StaloŽaidimas", b =>
                {
                    b.Property<int>("ŽaidimoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ŽaidimoId"));

                    b.Property<string>("Kategorija")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Pavadinimas")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

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
