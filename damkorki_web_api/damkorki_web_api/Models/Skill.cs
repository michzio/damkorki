using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DamkorkiWebApi.Models
{
	public class Skill
	{
		[Key]
		public int SkillId { get; set; }
		public string Name { get; set; }

		public List<TutorSkill> TutorSkills { get; set; }

	}
}
