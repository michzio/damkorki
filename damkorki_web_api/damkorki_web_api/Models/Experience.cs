using System;
using System.ComponentModel.DataAnnotations;


namespace DamkorkiWebApi.Models
{
	public class Experience
	{
		#region Principal properties
		[Key]
		public int ExperienceId { get; set; }
		[Required]
		public int StartYear { get; set; }
		[Required]
		public int EndYear { get; set; }
		[Required]
		public string Description { get; set; }
		#endregion

		#region FK properties
		[Required]
		public int TutorId { get; set; }
		#endregion

		#region Navigation properties
		public Tutor Tutor { get; set; }
		#endregion

	}
}
