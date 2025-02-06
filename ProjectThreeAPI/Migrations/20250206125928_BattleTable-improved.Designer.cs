﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250206125928_BattleTable-improved")]
    partial class BattleTableimproved
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Battle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("NpcId")
                        .HasColumnType("int");

                    b.Property<int>("SaveId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NpcId");

                    b.HasIndex("SaveId");

                    b.ToTable("battles");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Deck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SaveId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SaveId");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Save", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountBoosters")
                        .HasColumnType("int");

                    b.Property<int>("CountDefeats")
                        .HasColumnType("int");

                    b.Property<int>("CountMatches")
                        .HasColumnType("int");

                    b.Property<int>("CountVictories")
                        .HasColumnType("int");

                    b.Property<int>("Gold")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PlayerLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("saves");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.SaveDeckEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("DeckId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("DeckId");

                    b.ToTable("SaveDeckEntries");
                });

            modelBuilder.Entity("ProjectThreeAPI.Models.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Power")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UpperHand")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("cards");
                });

            modelBuilder.Entity("ProjectThreeAPI.Models.Entities.Npc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("npcs");
                });

            modelBuilder.Entity("ProjectThreeAPI.Models.Entities.NpcDeckEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("NpcId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("NpcId");

                    b.ToTable("NpcDeckEntries");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Battle", b =>
                {
                    b.HasOne("ProjectThreeAPI.Models.Entities.Npc", "Npc")
                        .WithMany()
                        .HasForeignKey("NpcId");

                    b.HasOne("MedievalAutoBattler.Models.Entities.Save", "Save")
                        .WithMany()
                        .HasForeignKey("SaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Npc");

                    b.Navigation("Save");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Deck", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.Save", "Save")
                        .WithMany("Decks")
                        .HasForeignKey("SaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Save");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.SaveDeckEntry", b =>
                {
                    b.HasOne("ProjectThreeAPI.Models.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedievalAutoBattler.Models.Entities.Deck", "Deck")
                        .WithMany("SaveDeckEntries")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("ProjectThreeAPI.Models.Entities.NpcDeckEntry", b =>
                {
                    b.HasOne("ProjectThreeAPI.Models.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectThreeAPI.Models.Entities.Npc", "Npc")
                        .WithMany("Deck")
                        .HasForeignKey("NpcId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Npc");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Deck", b =>
                {
                    b.Navigation("SaveDeckEntries");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Save", b =>
                {
                    b.Navigation("Decks");
                });

            modelBuilder.Entity("ProjectThreeAPI.Models.Entities.Npc", b =>
                {
                    b.Navigation("Deck");
                });
#pragma warning restore 612, 618
        }
    }
}
