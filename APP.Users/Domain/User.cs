using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Domain;

namespace APP.Users.Domain
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

        [NotMapped]
        public List<int> SkillIds
        {
            get => UserSkills.Select(us => us.SkillId).ToList();
            set => UserSkills = value.Select(v => new UserSkill() { SkillId = v }).ToList();
        }

    }
}
