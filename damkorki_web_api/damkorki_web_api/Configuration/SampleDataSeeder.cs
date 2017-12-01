using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DamkorkiWebApi.Models;
using DamkorkiWebApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DamkorkiWebApi.Configuration 
{
    public class SampleDataSeeder
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager; 
        

        public SampleDataSeeder(IUnitOfWork unitOfWork,
                                RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;

            _roleManager = roleManager;
            _userManager = userManager; 
        }

        public async Task SeedSampleDataAsync()
        {
            // create sample Roles 
            if(_roleManager.Roles.Count() == 0 ) await CreateSampleRolesAsync();

            // create sample Users
            if(_userManager.Users.Count() == 0) await CreateSampleUsers();
            //if((await _unitOfWork.Users.CountAsync()) == 0) CreateSampleUsers(); 

            // create sample Persons
            if((await _unitOfWork.People.CountAsync()) == 0) CreateSamplePersons();

            // create sample Addresses
            if((await _unitOfWork.Addresses.CountAsync()) == 0) CreateSampleAddresses();

            // create sample Learners
            if((await _unitOfWork.Learners.CountAsync()) == 0) CreateSampleLearners();

            // create sample Tutors
            if((await _unitOfWork.Tutors.CountAsync()) == 0) CreateSampleTutors();

            // create sample Subjects
            if((await _unitOfWork.Subjects.CountAsync()) == 0) CreateSampleSubjects();

            // create sample Skills
            if((await _unitOfWork.Skills.CountAsync()) == 0) CreateSampleSkills();

            // create sample Experieneces
            if((await _unitOfWork.Experiences.CountAsync()) == 0) CreateSampleExperiences();

            // create sample LessonOffers
            if((await _unitOfWork.LessonOffers.CountAsync()) == 0) CreateSampleLessonOffers();
            
        }

         public async Task CreateSampleRolesAsync()
        {
            await CreateRoleAsync("Admin"); 
            await CreateRoleAsync("Moderator");
            await CreateRoleAsync("User");
        }
         private async Task CreateRoleAsync(string name) { 

            if( !(await _roleManager.RoleExistsAsync(name)) )
                await _roleManager.CreateAsync(new IdentityRole { Name = name});

        }

        public async Task CreateSampleUsers() { 

            // UserManager used instead of DbContext
            // to auto hash password and to auto generate Id

            // create admin user 
            var adminUser = new ApplicationUser() {
                // Id = Guid.NewGuid().ToString(), 
                UserName = "iwona.wojciechowska",
                Email = "iwona.wojeciechowska@live.com",
                RegistrationDate = DateTime.Now  
            };

            await _userManager.CreateAsync(adminUser, "Pass4Admin!");
            await _userManager.AddToRoleAsync(adminUser, "Admin");
            adminUser.EmailConfirmed = true;
            adminUser.LockoutEnabled = false; 

            // create moderator user
            var moderatorUser = new ApplicationUser() { 
                // Id = Guid.NewGuid().ToString(), 
                UserName = "agnieszka.wojciechowska",
                Email = "agnieszka.wojciechowska@live.com", 
                RegistrationDate = DateTime.Now, 
                EmailConfirmed = true, 
                LockoutEnabled = false
            };

            await _userManager.CreateAsync(moderatorUser, "PaSSword12!"); 
            await _userManager.AddToRoleAsync(moderatorUser, "Moderator"); 

            // create other users  
            var user1 = new ApplicationUser() { 
                //Id = Guid.NewGuid().ToString(), 
                UserName = "michzio", 
                Email = "michzio@hotmail.com", 
                RegistrationDate = DateTime.Now, 
                EmailConfirmed = true, 
                LockoutEnabled = false
            };

            await _userManager.CreateAsync(user1, "Pass4user1!");
            await _userManager.AddToRoleAsync(user1, "User");

            var user2 = new ApplicationUser() {
                // Id = Guid.NewGuid().ToString(),
                UserName = "anowak",
                Email = "anna.nowak@gmail.com",
                RegistrationDate = DateTime.Now, 
                EmailConfirmed = true, 
                LockoutEnabled = false
            };

            await _userManager.CreateAsync(user2, "Pass4user2!");
            await _userManager.AddToRoleAsync(user2, "User");

            var user3 = new ApplicationUser() {
                // Id = Guid.NewGuid().ToString(),
                UserName = "tkobus",
                Email = "tomasz.kobus@gmail.com",
                RegistrationDate = DateTime.Now,
                EmailConfirmed = true, 
                LockoutEnabled = false
            };

            await _userManager.CreateAsync(user3, "Pass4user3!");
            await _userManager.AddToRoleAsync(user3, "User"); 

            var user4 = new ApplicationUser() {
                // Id = Guid.NewGuid().ToString(),
                UserName = "fmarlewicz",
                Email = "filip.marlewicz@gmailcom",
                RegistrationDate = DateTime.Now,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            await _userManager.CreateAsync(user4, "Pass4user4!");             
            await _userManager.AddToRoleAsync(user4, "User");

            _unitOfWork.Complete();
        }

        public void CreateSamplePersons()
        {
            // create admin person
            var adminId = _unitOfWork.Users.Find(u => u.UserName == "iwona.wojciechowska")
                             .FirstOrDefault().Id;

            var adminPerson = new Person() { 
                FirstName = "Iwona", 
                LastName = "Wojciechowska", 
                Gender = Person.GenderType.Female,
                UserId = adminId, 
                Birthdate = new DateTime(1989, 11, 27, 19, 30, 00) 
            };

            _unitOfWork.People.Add(adminPerson);

            // create moderator person 
            var moderatorId = _unitOfWork.Users.Find(u => u.UserName == "agnieszka.wojciechowska")
                            .FirstOrDefault().Id; 

            var moderatorPerson = new Person() { 
                FirstName = "Agnieszka", 
                LastName = "Wojciechowska", 
                Gender = Person.GenderType.Female, 
                UserId = moderatorId, 
                Birthdate = new DateTime(1983, 08, 05, 00, 00, 00)
            }; 

            _unitOfWork.People.Add(moderatorPerson);

            // create users presons
            var user1Id = _unitOfWork.Users.Find(u => u.UserName == "michzio")
                            .FirstOrDefault().Id; 

            var user1Person = new Person() { 
                FirstName = "Michał", 
                LastName = "Ziobro", 
                Gender = Person.GenderType.Male,
                UserId = user1Id, 
                Birthdate = new DateTime(1988, 10, 03, 00, 00, 00)
            };  

            var user2Id = _unitOfWork.Users.Find(u => u.UserName == "anowak")
                            .FirstOrDefault().Id; 

            var user2Person = new Person() { 
                FirstName = "Anna", 
                LastName = "Nowak", 
                Gender = Person.GenderType.Female,
                UserId = user2Id, 
                Birthdate = new DateTime(1990, 11, 26, 00, 00, 00)
            };

            var user3Id = _unitOfWork.Users.Find(u => u.UserName == "tkobus")
                            .FirstOrDefault().Id;

            var user3Person = new Person() { 
                FirstName = "Tomasz", 
                LastName = "Kobus", 
                Gender = Person.GenderType.Male,
                UserId = user3Id, 
                Birthdate = new DateTime(1987, 05, 14, 00, 00, 00)
            };

            var user4Id = _unitOfWork.Users.Find(u => u.UserName == "fmarlewicz")
                            .FirstOrDefault().Id;

            var user4Person = new Person() { 
                FirstName = "Filip", 
                LastName = "Marlewicz", 
                Gender = Person.GenderType.Male,
                UserId = user4Id, 
                Birthdate = new DateTime(1992, 06, 30, 00, 00, 00)
            };

            _unitOfWork.People.AddRange(new [] { user1Person, user2Person, user3Person, user4Person });

            _unitOfWork.Complete();
        }
        
        public void CreateSampleAddresses() { 

            // create admin address 
             var admin = _unitOfWork.Users.Find(u => u.UserName == "iwona.wojciechowska")
                             .FirstOrDefault().Person;

            Address address0 = new Address { 
                Street = "Grodzka", 
                HomeNumber = "12/20",
                ZipCode = "30-100",
                City = "Kraków", 
                Voivodeship = "małopolska", 
                Country = "Poland"
            }; 

            _unitOfWork.Addresses.Add(address0); 
            admin.Address = address0; 
            _unitOfWork.Complete(); 

            // create moderator address 
            var moderator = _unitOfWork.Users.Find(u => u.UserName == "agnieszka.wojciechowska")
                            .FirstOrDefault().Person; 

            moderator.Address = address0;                 
            _unitOfWork.Complete();                 

            // create user1 address 

            var person1 = _unitOfWork.Users.Find(u => u.UserName == "michzio")
                            .FirstOrDefault().Person;

            Address address1 = new Address { 
                Street = "Kobierzynska", 
                HomeNumber = "54/20",
                ZipCode = "30-150",
                City = "Kraków", 
                Voivodeship = "małopolska", 
                Country = "Poland"
            }; 

             _unitOfWork.Addresses.Add(address1); 
            person1.Address = address1; 

            // create user2 address 

            var person2 = _unitOfWork.Users.Find(u => u.UserName == "anowak")
                            .FirstOrDefault().Person; 

             Address address2 = new Address { 
                Street = "Bobrzyńskiego", 
                HomeNumber = "154/25",
                ZipCode = "35-150",
                City = "Kraków", 
                Voivodeship = "małopolska", 
                Country = "Poland"
            }; 

             _unitOfWork.Addresses.Add(address2); 
            person2.Address = address2; 

            // create user3 address

            var person3 = _unitOfWork.Users.Find(u => u.UserName == "tkobus")
                            .FirstOrDefault().Person;

            Address address3 = new Address { 
                Street = "Krakowska", 
                HomeNumber = "18",
                ZipCode = "35-100",
                City = "Kraków", 
                Voivodeship = "małopolska", 
                Country = "Poland"
            };  

             _unitOfWork.Addresses.Add(address3); 
            person3.Address = address3; 

            // create user4 address 

            var person4 = _unitOfWork.Users.Find(u => u.UserName == "fmarlewicz")
                            .FirstOrDefault().Person;

            Address address4 = new Address { 
                Street = "Urzędnicza", 
                HomeNumber = "100",
                ZipCode = "35-250",
                City = "Kraków", 
                Voivodeship = "małopolska", 
                Country = "Poland"
            }; 

             _unitOfWork.Addresses.Add(address4); 
            person4.Address = address4; 

            _unitOfWork.Complete(); 
        }

        public void CreateSampleLearners() { 
            // TODO: add sample Learners 
        }

        public void CreateSampleTutors() { 

            // create tutor profiles for person1, person2 and person3 

            string user1Id = _unitOfWork.Users.Find(u => u.UserName == "michzio")
						   .FirstOrDefault().Id;
			int person1Id = _unitOfWork.People.Find(p => p.UserId == user1Id).FirstOrDefault().PersonId;

            Tutor tutor1 = new Tutor { 
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse rhoncus id nisi eu semper. Duis dolor quam, gravida sit amet dignissim eu, cursus in metus. Sed ac sapien eget neque tristique sagittis sit amet et massa. Quisque eu nisl vitae massa porttitor dictum. In feugiat a diam a dictum. In malesuada dolor mauris, a feugiat nisl porta sit amet. Nunc ut condimentum neque. Suspendisse potenti. Curabitur non eros nunc. Donec placerat lacinia gravida.",
                    Qualifications = "Quisque pellentesque risus pharetra quam commodo, ac fermentum neque porta.",
                    IsSuperTutor = false,
                    PersonId = person1Id  
            };

            _unitOfWork.Tutors.Add(tutor1);              

            string user2Id = _unitOfWork.Users.Find(u => u.UserName == "anowak")
						   .FirstOrDefault().Id;
			int person2Id = _unitOfWork.People.Find(p => p.UserId == user2Id).FirstOrDefault().PersonId;            
                        
            Tutor tutor2 = new Tutor { 
                    Description = "Nam non est et diam dignissim tincidunt. Mauris in ante vel sapien semper interdum ac vitae risus. Vivamus tincidunt nunc nulla, eget porttitor sapien suscipit id. ",
                    Qualifications = "Quisque pellentesque risus pharetra quam commodo, ac fermentum neque porta.",
                    IsSuperTutor = true,
                    PersonId = person2Id  
            };
            
            _unitOfWork.Tutors.Add(tutor2); 

            string user3Id = _unitOfWork.Users.Find(u => u.UserName == "tkobus")
						   .FirstOrDefault().Id;
			int person3Id = _unitOfWork.People.Find(p => p.UserId == user3Id).FirstOrDefault().PersonId;  
                        
            Tutor tutor3 = new Tutor { 
                    Description = "Nam non est et diam dignissim tincidunt. Mauris in ante vel sapien semper interdum ac vitae risus. Vivamus tincidunt nunc nulla, eget porttitor sapien suscipit id. ",
                    Qualifications = "Suspendisse porttitor sapien eu justo tincidunt, at maximus libero malesuada.",
                    IsSuperTutor = false,
                    PersonId = person3Id  
            };
            
            _unitOfWork.Tutors.Add(tutor3); 

            _unitOfWork.Complete(); 
        }

        public void CreateSampleSubjects() { 

                Subject math = new Subject { 
                    Name = "Mathematics"
                }; 

                _unitOfWork.Subjects.Add(math); 

                Subject foreignLanguage = new Subject { 
                    Name = "Foreign Language"
                };

                _unitOfWork.Subjects.Add(foreignLanguage); 

                _unitOfWork.Complete(); 

                 Subject english = new Subject { 
                    Name = "English",
                    SuperSubject = foreignLanguage
                };

                Subject french = new Subject { 
                    Name = "French", 
                    SuperSubject = foreignLanguage
                };

                _unitOfWork.Subjects.AddRange(new [] { english, french}); 
                _unitOfWork.Complete(); 

                Subject algebra = new Subject { 
                    Name = "Algebra", 
                    SuperSubject = math
                }; 

                _unitOfWork.Subjects.Add(algebra); 
                _unitOfWork.Complete(); 
        }

        public void CreateSampleSkills() { 

            Skill nativeFrench = new Skill { 
                Name = "Native French"
            };

            Skill nativeEnglish = new Skill { 
                Name = "Native English"
            };

            Skill Cprogramming = new Skill { 
                Name = "C programming"
            };

            _unitOfWork.Skills.AddRange(new [] { nativeFrench, nativeEnglish, Cprogramming }); 
            _unitOfWork.Complete();


			string user1Id = _unitOfWork.Users.Find(u => u.UserName == "michzio")
						   .FirstOrDefault().Id;
			int person1Id = _unitOfWork.People.Find(p => p.UserId == user1Id).FirstOrDefault().PersonId;
			Tutor tutor1 = _unitOfWork.Tutors.Find(t => t.PersonId == person1Id).FirstOrDefault();

            TutorSkill tutor1NativeEnglishSkill = new TutorSkill { 
                TutorId = tutor1.TutorId,
                SkillId = nativeEnglish.SkillId
            };

		    tutor1.TutorSkills = new List<TutorSkill>() { tutor1NativeEnglishSkill };  
            _unitOfWork.Complete();                
        }

        public void CreateSampleExperiences() {
		   string user1Id = _unitOfWork.Users.Find(u => u.UserName == "michzio")
						   .FirstOrDefault().Id;
		   int person1Id = _unitOfWork.People.Find(p => p.UserId == user1Id).FirstOrDefault().PersonId;
           int tutor1Id = _unitOfWork.Tutors.Find(t => t.PersonId == person1Id).FirstOrDefault().TutorId; 

            Experience tutor1Experience1 = new Experience { 
                StartYear = 2000, 
                EndYear = 1995, 
                Description = "English Summer Camp",
                TutorId = tutor1Id
            }; 

            Experience tutor1Experience2 = new Experience { 
                StartYear = 2005, 
                EndYear = 2010, 
                Description = "English Course in High School",
                TutorId = tutor1Id
            }; 

            _unitOfWork.Experiences.AddRange(new [] { tutor1Experience1, tutor1Experience2 }); 
            _unitOfWork.Complete(); 

        }

        public void CreateSampleLessonOffers() { 

           string user1Id = _unitOfWork.Users.Find(u => u.UserName == "michzio")
						   .FirstOrDefault().Id;
		   int person1Id = _unitOfWork.People.Find(p => p.UserId == user1Id).FirstOrDefault().PersonId;
           int tutor1Id = _unitOfWork.Tutors.Find(t => t.PersonId == person1Id).FirstOrDefault().TutorId; 


            int englishSubjectId = _unitOfWork.Subjects.Find(s => s.Name == "English").FirstOrDefault().SubjectId; 

            LessonOffer lessonOffer1 = new LessonOffer { 
                Title = "Native Speaker Funny English Lessons!",
                Description = "Nullam et vehicula mi, quis condimentum magna. Maecenas aliquet ipsum tempor pretium pellentesque. Aenean aliquam pharetra mauris, sit amet maximus tortor auctor tristique. ",
                Cost = 50,
                Type = LessonOffer.LessonType.AtLearner | LessonOffer.LessonType.AtTutor,
                Location = "50.0777904;19.9248933;2000",  
                Level = LessonOffer.LevelType.Advanced | LessonOffer.LevelType.UpperIntermediate,
                SubjectId = englishSubjectId, 
                TutorId = tutor1Id
            };

            _unitOfWork.LessonOffers.Add(lessonOffer1); 
            _unitOfWork.Complete(); 
        }
    }
}
