using APP.Users.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersDbController : ControllerBase
    {
        private readonly UsersDb _db;

        public UsersDbController(UsersDb db)
        {
            _db = db;
        }

        [HttpGet("Seed")]
        public IActionResult Seed()
        {
            var userSkills = _db.UserSkills.ToList();
            _db.UserSkills.RemoveRange(userSkills);
            var skills = _db.Skills.ToList();
            _db.Skills.RemoveRange(skills);
            var users = _db.Users.ToList();
            _db.Users.RemoveRange(users);
            var roles = _db.Roles.ToList();
            _db.Roles.RemoveRange(roles);

            if (users.Any() || roles.Any() || userSkills.Any() || skills.Any())
            {
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Users', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roles', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Skills', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('UserSkills', RESEED, 0)");
            }

            _db.Skills.Add(new Skill()
            {
                Name = "Developer"
            });
            _db.Skills.Add(new Skill()
            {
                Name = "Instructor"
            });

            _db.SaveChanges();

            _db.Roles.Add(new Role()
            {
                Name = "Admin",
                Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Name = "Çağıl",
                        Password = "admin",
                        Surname = "Alsaç",
                        UserName = "admin",
                        RegistrationDate = new DateTime(2025, 01, 13),
                        UserSkills = new List<UserSkill>()
                        {
                            new UserSkill()
                            {
                                SkillId = _db.Skills.SingleOrDefault(s => s.Name == "Developer").Id
                            },
                            new UserSkill()
                            {
                                SkillId = _db.Skills.SingleOrDefault(s => s.Name == "Instructor").Id
                            }
                        }
                    }
                }
            });
            _db.Roles.Add(new Role()
            {
                Name = "User",
                Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Name = "Leo",
                        Password = "user",
                        Surname = "Alsaç",
                        UserName = "user",
                        RegistrationDate = DateTime.Parse("01/24/2025", new CultureInfo("en-US"))
                    }
                }
            });

            _db.SaveChanges();

            return Ok("Database seed successful.");
        }
    }
}
