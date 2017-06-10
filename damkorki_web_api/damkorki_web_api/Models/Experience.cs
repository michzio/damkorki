using System;
using System.ComponentModel.DataAnnotations;


namespace DamkorkiWebApi.Models
{
	public class Experience
	{
		#region Principal properties
		[Key]
		public int ExperienceId { get; set; }
		public int StartYear { get; set; }
		public int EndYear { get; set; }
		public string Description { get; set; }
		#endregion

		#region FK properties
		public int TutorId { get; set; }
		#endregion

		#region Navigation properties
		public Tutor Tutor { get; set; }
		#endregion

	}
}
