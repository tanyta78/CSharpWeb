﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SocialNetwork.Data;
using System;

namespace SocialNetwork.Data.Migrations
{
    [DbContext(typeof(SocialNetworkDbContext))]
    [Migration("20171220122044_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SocialNetwork.Models.Friendship", b =>
                {
                    b.Property<int>("FromUserId");

                    b.Property<int>("ToUserId");

                    b.HasKey("FromUserId", "ToUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("SocialNetwork.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Age");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastTimeLoggedIn");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<byte[]>("ProfilePicture")
                        .HasMaxLength(1024);

                    b.Property<DateTime?>("RegisteredOn");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocialNetwork.Models.Friendship", b =>
                {
                    b.HasOne("SocialNetwork.Models.User", "FromUser")
                        .WithMany("FromFriends")
                        .HasForeignKey("FromUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SocialNetwork.Models.User", "ToUser")
                        .WithMany("ToFriends")
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
