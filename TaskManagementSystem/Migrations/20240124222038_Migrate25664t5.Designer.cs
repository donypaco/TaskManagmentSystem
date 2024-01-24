﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagementSystem.Data;

#nullable disable

namespace TaskManagementSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240124222038_Migrate25664t5")]
    partial class Migrate25664t5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TaskManagementSystem.Data.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CompletedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserAssignedId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserAssignedId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.TaskStatus", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatusId"), 1L, 1);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusId");

                    b.ToTable("TaskStatuses");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.User", b =>
                {
                    b.Property<int>("Id_User")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_User"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id_User");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.Task", b =>
                {
                    b.HasOne("TaskManagementSystem.Data.TaskStatus", "Status")
                        .WithMany("Tasks")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManagementSystem.Data.User", "UserAssigned")
                        .WithMany("TasksAssigned")
                        .HasForeignKey("UserAssignedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("UserAssigned");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.User", b =>
                {
                    b.HasOne("TaskManagementSystem.Data.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.TaskStatus", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("TaskManagementSystem.Data.User", b =>
                {
                    b.Navigation("TasksAssigned");
                });
#pragma warning restore 612, 618
        }
    }
}
