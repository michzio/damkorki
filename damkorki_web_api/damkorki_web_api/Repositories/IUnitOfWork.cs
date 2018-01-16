using System;
namespace DamkorkiWebApi.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IPeopleRepository People { get; }
		IAddressesRepository Addresses { get; }
		IProfilePhotosRepository ProfilePhotos { get; }
		IUsersRepository Users { get; }
		IFeedbacksRepository Feedbacks { get; }
		ILearnersRepository Learners { get; }
		ILessonOffersRepository LessonOffers { get; }
		IReservationsRepository Reservations { get; }
		ISubjectsRepository Subjects { get; }
		ITermsRepository Terms { get; }
		ITutorsRepository Tutors { get; }
		IExperiencesRepository Experiences { get; }
		ISkillsRepository Skills { get; }

		int Complete();

	}
}
