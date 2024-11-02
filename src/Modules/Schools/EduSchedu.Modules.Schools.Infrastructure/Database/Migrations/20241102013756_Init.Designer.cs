﻿// <auto-generated />
using System;
using EduSchedu.Modules.Schools.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduSchedu.Modules.Schools.Infrastructure.Database.Migrations
{
    [DbContext(typeof(SchoolsDbContext))]
    [Migration("20241102013756_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("schools")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.Class", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LanguageProficiencyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("SchoolId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LanguageProficiencyId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Classes", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.LanguageProficiency", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lvl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LanguageProficiencies", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.Lesson", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AssignedTeacher")
                        .HasColumnType("uuid")
                        .HasColumnName("AssignedTeacherId");

                    b.Property<Guid?>("ClassId")
                        .HasColumnType("uuid");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time without time zone");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Lessons", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.School", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("HeadmasterId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("Schools", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId")
                        .IsUnique();

                    b.ToTable("Schedules", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.ScheduleItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeOnly>("End")
                        .HasColumnType("time without time zone");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uuid");

                    b.Property<TimeOnly>("Start")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("ScheduleItems", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.SchoolUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SchoolUsers", "schools");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Student", b =>
                {
                    b.HasBaseType("EduSchedu.Modules.Schools.Domain.Users.SchoolUser");

                    b.ToTable("Students", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Teacher", b =>
                {
                    b.HasBaseType("EduSchedu.Modules.Schools.Domain.Users.SchoolUser");

                    b.ToTable("Teacher", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Headmaster", b =>
                {
                    b.HasBaseType("EduSchedu.Modules.Schools.Domain.Users.Teacher");

                    b.ToTable("Headmaster", "schools");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.Class", b =>
                {
                    b.HasOne("EduSchedu.Modules.Schools.Domain.Schools.LanguageProficiency", "LanguageProficiency")
                        .WithMany()
                        .HasForeignKey("LanguageProficiencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EduSchedu.Modules.Schools.Domain.Schools.School", null)
                        .WithMany("Classes")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsMany("EduSchedu.Shared.Abstractions.Kernel.ValueObjects.UserId", "StudentIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ClassId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("StudentId");

                            b1.HasKey("Id");

                            b1.HasIndex("ClassId");

                            b1.ToTable("ClassStudentIds", "schools");

                            b1.WithOwner()
                                .HasForeignKey("ClassId");
                        });

                    b.Navigation("LanguageProficiency");

                    b.Navigation("StudentIds");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.Lesson", b =>
                {
                    b.HasOne("EduSchedu.Modules.Schools.Domain.Schools.Class", null)
                        .WithMany("Lessons")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.School", b =>
                {
                    b.OwnsMany("EduSchedu.Shared.Abstractions.Kernel.ValueObjects.UserId", "StudentIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("SchoolId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("StudentId");

                            b1.HasKey("Id");

                            b1.HasIndex("SchoolId");

                            b1.ToTable("SchoolStudentIds", "schools");

                            b1.WithOwner()
                                .HasForeignKey("SchoolId");
                        });

                    b.OwnsMany("EduSchedu.Shared.Abstractions.Kernel.ValueObjects.UserId", "TeacherIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("SchoolId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("TeacherId");

                            b1.HasKey("Id");

                            b1.HasIndex("SchoolId");

                            b1.ToTable("SchoolTeacherIds", "schools");

                            b1.WithOwner()
                                .HasForeignKey("SchoolId");
                        });

                    b.OwnsOne("EduSchedu.Shared.Abstractions.Kernel.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("SchoolId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("City");

                            b1.Property<string>("MapCoordinates")
                                .HasColumnType("text")
                                .HasColumnName("MapCoordinates");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("Street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("ZipCode");

                            b1.HasKey("SchoolId");

                            b1.ToTable("Schools", "schools");

                            b1.WithOwner()
                                .HasForeignKey("SchoolId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("StudentIds");

                    b.Navigation("TeacherIds");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Schedule", b =>
                {
                    b.HasOne("EduSchedu.Modules.Schools.Domain.Users.Teacher", "Teacher")
                        .WithOne("Schedule")
                        .HasForeignKey("EduSchedu.Modules.Schools.Domain.Users.Schedule", "TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.ScheduleItem", b =>
                {
                    b.HasOne("EduSchedu.Modules.Schools.Domain.Users.Schedule", null)
                        .WithMany("ScheduleItems")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Student", b =>
                {
                    b.HasOne("EduSchedu.Modules.Schools.Domain.Users.SchoolUser", null)
                        .WithOne()
                        .HasForeignKey("EduSchedu.Modules.Schools.Domain.Users.Student", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Teacher", b =>
                {
                    b.HasOne("EduSchedu.Modules.Schools.Domain.Users.SchoolUser", null)
                        .WithOne()
                        .HasForeignKey("EduSchedu.Modules.Schools.Domain.Users.Teacher", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("EduSchedu.Modules.Schools.Domain.Schools.Ids.LanguageProficiencyId", "LanguageProficiencyIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("TeacherId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("LanguageProficiencyId");

                            b1.HasKey("Id");

                            b1.HasIndex("TeacherId");

                            b1.ToTable("TeacherLanguageProficiencyIds", "schools");

                            b1.WithOwner()
                                .HasForeignKey("TeacherId");
                        });

                    b.Navigation("LanguageProficiencyIds");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.Class", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Schools.School", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Schedule", b =>
                {
                    b.Navigation("ScheduleItems");
                });

            modelBuilder.Entity("EduSchedu.Modules.Schools.Domain.Users.Teacher", b =>
                {
                    b.Navigation("Schedule")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
