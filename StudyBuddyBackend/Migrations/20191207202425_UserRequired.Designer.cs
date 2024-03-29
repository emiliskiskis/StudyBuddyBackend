﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StudyBuddyBackend.Database;

namespace StudyBuddyBackend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20191207202425_UserRequired")]
    partial class UserRequired
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.Chat", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.Feedback", b =>
                {
                    b.Property<string>("AuthorUsername")
                        .HasColumnType("character varying");

                    b.Property<string>("ReviewerUsername")
                        .HasColumnType("character varying");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.HasKey("AuthorUsername", "ReviewerUsername");

                    b.HasIndex("ReviewerUsername");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.Message", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("character varying");

                    b.Property<string>("ChatId")
                        .HasColumnType("text");

                    b.Property<DateTime>("SentAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Username", "ChatId", "SentAt");

                    b.HasIndex("ChatId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.ProfilePicture", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("character varying");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.HasKey("Username");

                    b.ToTable("ProfilePictures");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.Subject", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("VARCHAR");

                    b.HasKey("Name");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(255);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(255);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(255);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("VARCHAR")
                        .HasMaxLength(255);

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.UserInChat", b =>
                {
                    b.Property<string>("ChatId")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("character varying");

                    b.HasKey("ChatId", "Username");

                    b.HasIndex("Username");

                    b.ToTable("UsersInChats");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.UserSubject", b =>
                {
                    b.Property<string>("SubjectName")
                        .HasColumnType("character varying");

                    b.Property<string>("Username")
                        .HasColumnType("character varying");

                    b.HasKey("SubjectName", "Username");

                    b.HasIndex("Username");

                    b.ToTable("UserSubjects");
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.Feedback", b =>
                {
                    b.HasOne("StudyBuddyBackend.Database.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudyBuddyBackend.Database.Entities.User", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.Message", b =>
                {
                    b.HasOne("StudyBuddyBackend.Database.Entities.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudyBuddyBackend.Database.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.ProfilePicture", b =>
                {
                    b.HasOne("StudyBuddyBackend.Database.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.UserInChat", b =>
                {
                    b.HasOne("StudyBuddyBackend.Database.Entities.Chat", "Chat")
                        .WithMany("Users")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudyBuddyBackend.Database.Entities.User", "User")
                        .WithMany("Chats")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudyBuddyBackend.Database.Entities.UserSubject", b =>
                {
                    b.HasOne("StudyBuddyBackend.Database.Entities.Subject", "Subject")
                        .WithMany("Users")
                        .HasForeignKey("SubjectName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudyBuddyBackend.Database.Entities.User", "User")
                        .WithMany("Subjects")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
