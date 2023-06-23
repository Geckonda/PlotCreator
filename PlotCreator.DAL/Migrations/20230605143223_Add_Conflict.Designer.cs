﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlotCreator.DAL;

#nullable disable

namespace PlotCreator.DAL.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230605143223_Add_Conflict")]
    partial class Add_Conflict
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PlotCreator.Domain.Entity.Access_Modificator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Modificators");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Book_cover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("NText");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<int>("ModificatorId")
                        .HasColumnType("int");

                    b.Property<int>("RatingId")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("ModificatorId");

                    b.HasIndex("RatingId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Book_Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Appearance")
                        .HasColumnType("NText");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("Date");

                    b.Property<string>("Conflict")
                        .HasColumnType("NText");

                    b.Property<DateTime>("Deathday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Goals")
                        .HasColumnType("NText");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("History")
                        .HasColumnType("NText");

                    b.Property<string>("Motivation")
                        .HasColumnType("NText");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Personality")
                        .HasColumnType("NText");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("WorldviewId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WorldviewId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Episode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("NText");

                    b.Property<string>("Heading")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Beginning")
                        .HasColumnType("datetime2");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<bool>("ChekhovsGun")
                        .HasColumnType("bit");

                    b.Property<string>("Colour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("NText");

                    b.Property<DateTime>("Ending")
                        .HasColumnType("datetime2");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("Ntext");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parent")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Idea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("NText");

                    b.Property<DateTime>("Data_Creation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ideas");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Book_Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("CharacterId");

                    b.ToTable("Book-Character");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Book_Idea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("IdeaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("IdeaId");

                    b.ToTable("Book-Idea");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Episode_Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<int>("EpisodeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("EpisodeId");

                    b.ToTable("Episode-Character");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Episode_Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EpisodeId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EpisodeId");

                    b.HasIndex("EventId");

                    b.ToTable("Episode-Event");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Event_Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("EventId");

                    b.ToTable("Event-Character");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Group_Character", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CharacterId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("GroupId");

                    b.ToTable("Group-Character");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Group_Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("GroupId");

                    b.ToTable("Group-Event");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("roleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Worldview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("NText");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Worldview");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Book", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Genre", "Genre")
                        .WithMany("Books")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Access_Modificator", "Modificator")
                        .WithMany("Books")
                        .HasForeignKey("ModificatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Rating", "Rating")
                        .WithMany("Books")
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Book_Status", "Status")
                        .WithMany("Books")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.User", "User")
                        .WithMany("Books")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Modificator");

                    b.Navigation("Rating");

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Character", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.User", "User")
                        .WithMany("Characters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Worldview", "Worldview")
                        .WithMany("Characters")
                        .HasForeignKey("WorldviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Worldview");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Episode", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Book", "Book")
                        .WithMany("Episodes")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Event", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Book", "Book")
                        .WithMany("Events")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Group", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Book", "Book")
                        .WithMany("Groups")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Idea", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.User", "User")
                        .WithMany("Ideas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Book_Character", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Book", "Book")
                        .WithMany("Books_Characters")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Character", "Character")
                        .WithMany("Books_Characters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Book_Idea", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Book", "Book")
                        .WithMany("Books_Ideas")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Idea", "Idea")
                        .WithMany("Books_Ideas")
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Idea");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Episode_Character", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Character", "Character")
                        .WithMany("Episodes_Characters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Episode", "Episode")
                        .WithMany("Episodes_Characters")
                        .HasForeignKey("EpisodeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Episode");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Episode_Event", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Episode", "Episode")
                        .WithMany("Episodes_Events")
                        .HasForeignKey("EpisodeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Event", "Event")
                        .WithMany("Episodes_Events")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Episode");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Event_Character", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Character", "Character")
                        .WithMany("Events_Characters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Event", "Event")
                        .WithMany("Events_Characters")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Group_Character", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Character", "Character")
                        .WithMany("Groups_Characters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Group", "Group")
                        .WithMany("Groups_Characters")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Multiple_Tables.Group_Event", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Event", "Event")
                        .WithMany("Groups_Events")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PlotCreator.Domain.Entity.Group", "Group")
                        .WithMany("Groups_Events")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.User", b =>
                {
                    b.HasOne("PlotCreator.Domain.Entity.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Access_Modificator", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Book", b =>
                {
                    b.Navigation("Books_Characters");

                    b.Navigation("Books_Ideas");

                    b.Navigation("Episodes");

                    b.Navigation("Events");

                    b.Navigation("Groups");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Book_Status", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Character", b =>
                {
                    b.Navigation("Books_Characters");

                    b.Navigation("Episodes_Characters");

                    b.Navigation("Events_Characters");

                    b.Navigation("Groups_Characters");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Episode", b =>
                {
                    b.Navigation("Episodes_Characters");

                    b.Navigation("Episodes_Events");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Event", b =>
                {
                    b.Navigation("Episodes_Events");

                    b.Navigation("Events_Characters");

                    b.Navigation("Groups_Events");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Genre", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Group", b =>
                {
                    b.Navigation("Groups_Characters");

                    b.Navigation("Groups_Events");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Idea", b =>
                {
                    b.Navigation("Books_Ideas");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Rating", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.User", b =>
                {
                    b.Navigation("Books");

                    b.Navigation("Characters");

                    b.Navigation("Ideas");
                });

            modelBuilder.Entity("PlotCreator.Domain.Entity.Worldview", b =>
                {
                    b.Navigation("Characters");
                });
#pragma warning restore 612, 618
        }
    }
}
