﻿using System;
using MementoMori.Server.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MementoMori.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MementoMori.Server.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("DeckId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DeckId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("MementoMori.Server.Deck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("CardCount")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateOnly>("Modified")
                        .HasColumnType("date");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.Property<long>("RatingCount")
                        .HasColumnType("bigint");

                    b.Property<int[]>("Tags")
                        .HasColumnType("integer[]");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isPublic")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("MementoMori.Server.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MementoMori.Server.Models.UserCardData", b =>
                {
                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DeckId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<double>("EaseFactor")
                        .HasColumnType("double precision");

                    b.Property<int>("Interval")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastReviewed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Repetitions")
                        .HasColumnType("integer");

                    b.HasKey("CardId", "DeckId", "UserId");

                    b.ToTable("UserCards");
                });

            modelBuilder.Entity("MementoMori.Server.Card", b =>
                {
                    b.HasOne("MementoMori.Server.Deck", null)
                        .WithMany("Cards")
                        .HasForeignKey("DeckId");
                });

            modelBuilder.Entity("MementoMori.Server.Deck", b =>
                {
                    b.HasOne("MementoMori.Server.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("MementoMori.Server.Deck", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
