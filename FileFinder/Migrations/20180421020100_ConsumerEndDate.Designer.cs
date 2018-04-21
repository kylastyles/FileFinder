﻿// <auto-generated />
using FileFinder.Data;
using FileFinder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace FileFinder.Migrations
{
    [DbContext(typeof(FileFinderContext))]
    [Migration("20180421020100_ConsumerEndDate")]
    partial class ConsumerEndDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FileFinder.Models.Building", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Name");

                    b.Property<int>("PhoneNumber");

                    b.HasKey("ID");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("FileFinder.Models.CaseManager", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("ProgramID");

                    b.HasKey("ID");

                    b.HasIndex("ProgramID");

                    b.ToTable("CaseManagers");
                });

            modelBuilder.Entity("FileFinder.Models.Consumer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DOB");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Consumers");
                });

            modelBuilder.Entity("FileFinder.Models.File", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CaseManagerID");

                    b.Property<int>("ConsumerID");

                    b.Property<int>("Quantity");

                    b.Property<int>("RoomID");

                    b.Property<DateTime?>("ShredDate");

                    b.Property<int?>("Status");

                    b.HasKey("ID");

                    b.HasIndex("CaseManagerID");

                    b.HasIndex("ConsumerID");

                    b.HasIndex("RoomID");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("FileFinder.Models.Program", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("FileFinder.Models.Room", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuildingID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("BuildingID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("FileFinder.Models.CaseManager", b =>
                {
                    b.HasOne("FileFinder.Models.Program", "Program")
                        .WithMany("CaseManagers")
                        .HasForeignKey("ProgramID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileFinder.Models.File", b =>
                {
                    b.HasOne("FileFinder.Models.CaseManager", "CaseManager")
                        .WithMany("Files")
                        .HasForeignKey("CaseManagerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileFinder.Models.Consumer", "Consumer")
                        .WithMany("Files")
                        .HasForeignKey("ConsumerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FileFinder.Models.Room", "Room")
                        .WithMany("Files")
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FileFinder.Models.Room", b =>
                {
                    b.HasOne("FileFinder.Models.Building", "Building")
                        .WithMany("Rooms")
                        .HasForeignKey("BuildingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
