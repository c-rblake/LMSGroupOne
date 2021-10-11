﻿// <auto-generated />
using System;
using LMS.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LMS.Api.Migrations
{
    [DbContext(typeof(LMSApiContext))]
    [Migration("20211011112849_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuthorWork", b =>
                {
                    b.Property<int>("AuthorsId")
                        .HasColumnType("int");

                    b.Property<int>("WorksId")
                        .HasColumnType("int");

                    b.HasKey("AuthorsId", "WorksId");

                    b.HasIndex("WorksId");

                    b.ToTable("AuthorWork");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorWorkAuthorId")
                        .HasColumnType("int");

                    b.Property<int?>("AuthorWorkWorkId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorWorkAuthorId", "AuthorWorkWorkId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.AuthorWork", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("WorkId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId", "WorkId");

                    b.ToTable("AuthorWorks");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorWorkAuthorId")
                        .HasColumnType("int");

                    b.Property<int?>("AuthorWorkWorkId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("Level")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("TypeId");

                    b.HasIndex("AuthorWorkAuthorId", "AuthorWorkWorkId");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("AuthorWork", b =>
                {
                    b.HasOne("LMS.Api.Core.Entities.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS.Api.Core.Entities.Work", null)
                        .WithMany()
                        .HasForeignKey("WorksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.Author", b =>
                {
                    b.HasOne("LMS.Api.Core.Entities.AuthorWork", null)
                        .WithMany("Authors")
                        .HasForeignKey("AuthorWorkAuthorId", "AuthorWorkWorkId");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.Work", b =>
                {
                    b.HasOne("LMS.Api.Core.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS.Api.Core.Entities.Type", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMS.Api.Core.Entities.AuthorWork", null)
                        .WithMany("Works")
                        .HasForeignKey("AuthorWorkAuthorId", "AuthorWorkWorkId");

                    b.Navigation("Genre");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("LMS.Api.Core.Entities.AuthorWork", b =>
                {
                    b.Navigation("Authors");

                    b.Navigation("Works");
                });
#pragma warning restore 612, 618
        }
    }
}
