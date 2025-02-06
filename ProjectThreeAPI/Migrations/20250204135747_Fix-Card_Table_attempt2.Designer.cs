﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250204135747_Fix-Card_Table_attempt2")]
    partial class FixCard_Table_attempt2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Save", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountBoosters")
                        .HasColumnType("int");

                    b.Property<int>("CountLosses")
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

                    b.HasKey("Id");

                    b.ToTable("saves");
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

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.DeckEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("NpcId")
                        .HasColumnType("int");

                    b.Property<int?>("SaveId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("NpcId");

                    b.HasIndex("SaveId");

                    b.ToTable("DeckEntries");
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

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.DeckEntry", b =>
                {
                    b.HasOne("MedievalAutoBattler.Models.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedievalAutoBattler.Models.Entities.Npc", "Npc")
                        .WithMany("Deck")
                        .HasForeignKey("NpcId");

                    b.HasOne("MedievalAutoBattler.Models.Entities.Save", "Save")
                        .WithMany("Deck")
                        .HasForeignKey("SaveId");

                    b.Navigation("Card");

                    b.Navigation("Npc");

                    b.Navigation("Save");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Save", b =>
                {
                    b.Navigation("Deck");
                });

            modelBuilder.Entity("MedievalAutoBattler.Models.Entities.Npc", b =>
                {
                    b.Navigation("Deck");
                });
#pragma warning restore 612, 618
        }
    }
}
