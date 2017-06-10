using System;
namespace DamkorkiWebApi.Models
{
	public class TutorSkill
	{
		// FK Properties
		public int TutorId { get; set; }
		public int SkillId { get; set; }

		// Navigation Properties
		public Tutor Tutor { get; set; }
		public Skill Skill { get; set; }	

	}
}
