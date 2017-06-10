using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170429180755_PersonsTableNameChangedToUpperCase")]
    partial class PersonsTableNameChangedToUpperCase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DamkorkiWebApi.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("HomeNumber")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Voivodeship")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.HasKey("AddressId");

                    b.ToTable("address");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<DateTime>("FailedLoginDate");

                    b.Property<DateTime>("LastLoginDate");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<string>("Demmenti");

                    b.Property<int>("Rating")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(3)
                        .HasMaxLength(1);

                    b.Property<int>("ReservationId");

                    b.HasKey("FeedbackId");

                    b.HasIndex("ReservationId")
                        .IsUnique();

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Learner", b =>
                {
                    b.Property<int>("LearnerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("PersonId");

                    b.HasKey("LearnerId");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Learners");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.LessonOffer", b =>
                {
                    b.Property<int>("LessonOfferId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cost");

                    b.Property<string>("Description");

                    b.Property<int>("Level");

                    b.Property<string>("Location")
                        .IsRequired();

                    b.Property<int>("SubjectId");

                    b.Property<int>("TutorId");

                    b.Property<int>("Type");

                    b.HasKey("LessonOfferId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TutorId");

                    b.ToTable("LessonOffers");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.LessonOfferTerm", b =>
                {
                    b.Property<int>("LessonOfferId");

                    b.Property<int>("TermId");

                    b.HasKey("LessonOfferId", "TermId");

                    b.HasIndex("TermId");

                    b.ToTable("LessonOfferTerm");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressId");

                    b.Property<int?>("Age");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasMaxLength(50);

                    b.Property<int>("Gender");

                    b.Property<string>("Image");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name")
                        .HasMaxLength(50);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("PersonId");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("People");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("LearnerId");

                    b.Property<int>("LessonOfferId");

                    b.Property<int>("ReservationNumber");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TermId");

                    b.Property<int>("TutorId");

                    b.HasKey("ReservationId");

                    b.HasAlternateKey("ReservationNumber")
                        .HasName("AK_ReservationNumber");

                    b.HasIndex("LearnerId");

                    b.HasIndex("LessonOfferId");

                    b.HasIndex("TermId");

                    b.HasIndex("TutorId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("SuperSubjectId");

                    b.HasKey("SubjectId");

                    b.HasIndex("SuperSubjectId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Term", b =>
                {
                    b.Property<int>("TermId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<DateTime>("Start");

                    b.HasKey("TermId");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Tutor", b =>
                {
                    b.Property<int>("TutorId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsSuperTutor");

                    b.Property<int>("PersonId");

                    b.Property<string>("Qualifications");

                    b.HasKey("TutorId");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Tutors");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Feedback", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Reservation", "Reservation")
                        .WithOne("Feedback")
                        .HasForeignKey("DamkorkiWebApi.Models.Feedback", "ReservationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Learner", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Person", "Person")
                        .WithOne("Learner")
                        .HasForeignKey("DamkorkiWebApi.Models.Learner", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.LessonOffer", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Subject", "Subject")
                        .WithMany("LessonOffers")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DamkorkiWebApi.Models.Tutor", "Tutor")
                        .WithMany("LessonOffers")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.LessonOfferTerm", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.LessonOffer", "LessonOffer")
                        .WithMany("LessonOfferTerms")
                        .HasForeignKey("LessonOfferId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DamkorkiWebApi.Models.Term", "Term")
                        .WithMany("LessonOfferTerms")
                        .HasForeignKey("TermId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Person", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Address", "Address")
                        .WithMany("People")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DamkorkiWebApi.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Person")
                        .HasForeignKey("DamkorkiWebApi.Models.Person", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Reservation", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Learner", "Learner")
                        .WithMany("Reservations")
                        .HasForeignKey("LearnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DamkorkiWebApi.Models.LessonOffer", "LessonOffer")
                        .WithMany("Reservations")
                        .HasForeignKey("LessonOfferId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DamkorkiWebApi.Models.Term", "Term")
                        .WithMany("Reservations")
                        .HasForeignKey("TermId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DamkorkiWebApi.Models.Tutor", "Tutor")
                        .WithMany("Reservations")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Subject", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Subject", "SuperSubject")
                        .WithMany("SubSubjects")
                        .HasForeignKey("SuperSubjectId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("DamkorkiWebApi.Models.Tutor", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.Person", "Person")
                        .WithOne("Tutor")
                        .HasForeignKey("DamkorkiWebApi.Models.Tutor", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DamkorkiWebApi.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DamkorkiWebApi.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
