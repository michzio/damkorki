using System;
using DamkorkiWebApi.Models;

namespace DamkorkiWebApi.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DatabaseContext _context;

		public UnitOfWork(DatabaseContext context)
		{
			_context = context;
			People = new PeopleRepository(_context);
			Addresses = new AddressesRepository(_context);
			ProfilePhotos = new ProfilePhotosRepository(_context); 
			Users = new UsersRepository(_context);
			Feedbacks = new FeedbacksRepository(_context);
			Learners = new LearnersRepository(_context);
			LessonOffers = new LessonOffersRepository(_context);
			Reservations = new ReservationsRepository(_context);
			Subjects = new SubjectsRepository(_context);
			Terms = new TermsRepository(_context);
			Tutors = new TutorsRepository(_context);
			Experiences = new ExperiencesRepository(_context);
			Skills = new SkillsRepository(_context); 
			TutorsSkills = new TutorsSkillsRepository(_context); 
		}

		public IPeopleRepository People { get; private set; }
		public IAddressesRepository Addresses { get; private set; }
		public IProfilePhotosRepository ProfilePhotos { get; private set; }
		public IUsersRepository Users { get; private set; }
		public IFeedbacksRepository Feedbacks { get; private set; }
		public ILearnersRepository Learners { get; private set; }
		public ILessonOffersRepository LessonOffers { get; private set; }
		public IReservationsRepository Reservations { get; private set; }
		public ISubjectsRepository Subjects { get; private set; }
		public ITermsRepository Terms { get; private set; }
		public ITutorsRepository Tutors { get; private set; }
		public IExperiencesRepository Experiences { get; private set; }
		public ISkillsRepository Skills { get; private set; }
		public ITutorsSkillsRepository TutorsSkills { get; private set; }

		public int Complete()
		{
			return _context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
