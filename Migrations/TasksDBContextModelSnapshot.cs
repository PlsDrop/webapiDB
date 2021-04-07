﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using webapi;

namespace webapi.Migrations
{
    [DbContext(typeof(TasksDBContext))]
    partial class TasksDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("webapi.Task", b =>
                {
                    b.Property<int>("taskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("task_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("done")
                        .HasColumnType("boolean")
                        .HasColumnName("done");

                    b.Property<DateTime?>("dueDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("due_date");

                    b.Property<int>("tasksListId")
                        .HasColumnType("integer")
                        .HasColumnName("tasks_list_id");

                    b.Property<string>("title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("taskId")
                        .HasName("pk_tasks");

                    b.HasIndex("tasksListId")
                        .HasDatabaseName("ix_tasks_tasks_list_id");

                    b.ToTable("tasks");
                });

            modelBuilder.Entity("webapi.TasksList", b =>
                {
                    b.Property<int>("tasksListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("tasks_list_id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("title")
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("tasksListId")
                        .HasName("pk_task_lists");

                    b.ToTable("task_lists");
                });

            modelBuilder.Entity("webapi.Task", b =>
                {
                    b.HasOne("webapi.TasksList", "tasksList")
                        .WithMany("tasks")
                        .HasForeignKey("tasksListId")
                        .HasConstraintName("fk_tasks_task_lists_tasks_list_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("tasksList");
                });

            modelBuilder.Entity("webapi.TasksList", b =>
                {
                    b.Navigation("tasks");
                });
#pragma warning restore 612, 618
        }
    }
}