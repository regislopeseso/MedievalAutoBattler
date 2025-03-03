﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<bool>("IsFinished")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("NpcId")
                        .HasColumnType("int");

                    b.Property<string>("Results")
                        .HasColumnType("longtext");

                    b.Property<int>("SaveId")
                        .HasColumnType("int");

                    b.Property<string>("Winner")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("NpcId");

                    b.HasIndex("SaveId");

                    b.ToTable("battles");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Card", b =>
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

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Npc", b =>
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

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.NpcsDeckEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("NpcId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("NpcId");

                    b.ToTable("NpcDeckEntries");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.PlayersCardEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SaveId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("SaveId");

                    b.ToTable("PlayerCardEntries");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.PlayersDeckEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DeckId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SaveCardEntryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeckId");

                    b.HasIndex("SaveCardEntryId");

                    b.ToTable("PlayerDeckEntries");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.PlayersSave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AllCardsCollectedTrophy")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("AllNpcsDefeatedTrophy")
                        .HasColumnType("tinyint(1)");

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

                    b.ToTable("PlayersSaves");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Battle", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.Npc", "Npc")
                        .WithMany()
                        .HasForeignKey("NpcId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedievalAutoBattler.Models.Entities.PlayersSave", "Save")
                        .WithMany()
                        .HasForeignKey("SaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Npc");

                    b.Navigation("Save");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Deck", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.PlayersSave", "Save")
                        .WithMany("Decks")
                        .HasForeignKey("SaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Save");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.NpcsDeckEntry", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedievalAutoBattler.Models.Entities.Npc", "Npc")
                        .WithMany("Deck")
                        .HasForeignKey("NpcId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Npc");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.PlayersCardEntry", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedievalAutoBattler.Models.Entities.PlayersSave", "Save")
                        .WithMany("SaveCardEntries")
                        .HasForeignKey("SaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Save");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.PlayersDeckEntry", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.Deck", "Deck")
                        .WithMany("SaveDeckEntries")
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedievalAutoBattler.Models.Entities.PlayersCardEntry", "SaveCardEntry")
                        .WithMany()
                        .HasForeignKey("SaveCardEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Deck");

                    b.Navigation("SaveCardEntry");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Deck", b =>
                {
                    b.Navigation("SaveDeckEntries");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Npc", b =>
                {
                    b.Navigation("Deck");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.PlayersSave", b =>
                {
                    b.Navigation("Decks");

                    b.Navigation("SaveCardEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
