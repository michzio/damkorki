using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DamkorkiWebApi.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        public DbSet<Person> People { get; set; }
        public DbSet<Learner> Learners { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<LessonOffer> LessonOffers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Term> Terms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<Person>().HasKey(p => p.UserId).HasName(PK_UserId); Fluent API primary key definition
            // modelBuilder.Entity<Person>(e => e.Property(person => person.LastName).HasColumnName("last_name") ); Fluent Api custom column name

            modelBuilder.Entity<Person>()
                .HasOne(p => p.ApplicationUser)
                .WithOne(au => au.Person)
                .HasForeignKey<Person>(p => p.UserId)
                .HasPrincipalKey<ApplicationUser>(au => au.Id);
                

            modelBuilder.Entity<Address>().ToTable("Address");

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Address)
                .WithMany(a => a.People)
                .HasForeignKey(p => p.AddressId)
                .HasPrincipalKey(a => a.AddressId)
            // .IsRequired() 
               .OnDelete(DeleteBehavior.SetNull);  // Cascade, Restrict, SetNull 

            // modelBuilder.Entity<Person>().HasIndex(p => new { p.FirstName, p.LastName }).IsUnique();
            // modelBuilder.Entity<Person>().HasAlternateKey(p => new { p.FirstName, p.LastName });

            modelBuilder.Entity<Address>()
                .Property(a => a.ZipCode)
                .HasField("_validatedZipCode");

            modelBuilder.Entity<Learner>()
                .HasOne(l => l.Person)
                .WithOne(p => p.Learner)
                .HasForeignKey<Learner>(l => l.PersonId);

            modelBuilder.Entity<Tutor>()
                .HasOne(t => t.Person)
                .WithOne(p => p.Tutor)
                .HasForeignKey<Tutor>(t => t.PersonId);

            modelBuilder.Entity<Feedback>()
                .Property(f => f.Rating)
                .HasDefaultValue(3);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Reservation)
                .WithOne(r => r.Feedback)
                .HasForeignKey<Feedback>(f => f.ReservationId);

            modelBuilder.Entity<Reservation>()
                .HasAlternateKey(r => r.ReservationNumber)
                .HasName("AK_ReservationNumber");

            modelBuilder.Entity<Subject>()
                .HasOne(sub => sub.SuperSubject)
                .WithMany(super => super.SubSubjects)
                .HasForeignKey(sub => sub.SuperSubjectId)
                .HasPrincipalKey(super => super.SubjectId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LessonOffer>()
                .HasOne(lo => lo.Subject)
                .WithMany(s => s.LessonOffers)
                .HasForeignKey(lo => lo.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LessonOffer>()
                .HasOne(lo => lo.Tutor)
                .WithMany(t => t.LessonOffers)
                .HasForeignKey(lo => lo.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LessonOfferTerm>()
                .HasKey(lot => new { lot.LessonOfferId, lot.TermId });

            modelBuilder.Entity<LessonOfferTerm>()
                 .HasOne(lot => lot.Term)
                 .WithMany(t => t.LessonOfferTerms)
                 .HasForeignKey(lot => lot.TermId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LessonOfferTerm>()
                .HasOne(lot => lot.LessonOffer)
                .WithMany(lo => lo.LessonOfferTerms)
                .HasForeignKey(lot => lot.LessonOfferId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Learner)
                .WithMany(l => l.Reservations)
                .HasForeignKey(r => r.LearnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Tutor)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.LessonOffer)
                .WithMany(lo => lo.Reservations)
                .HasForeignKey(r => r.LessonOfferId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Term)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TermId)
                .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Experience>()
				    .HasOne(e => e.Tutor)
				    .WithMany(t => t.Experiences)
				    .HasForeignKey(e => e.TutorId)
				    .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<TutorSkill>()
			            .HasKey(ts => new { ts.TutorId, ts.SkillId });

			modelBuilder.Entity<TutorSkill>()
				    .HasOne(ts => ts.Skill)
				    .WithMany(s => s.TutorSkills)
				    .HasForeignKey(ts => ts.SkillId)
			            .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<TutorSkill>()
			            .HasOne(ts => ts.Tutor)
			            .WithMany(t => t.TutorSkills)
			            .HasForeignKey(ts => ts.TutorId)
			            .OnDelete(DeleteBehavior.Cascade);
        }

    }
}

