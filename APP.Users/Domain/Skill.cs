using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Domain;

namespace APP.Users.Domain
{
    public class Skill : Entity
    {
        [Required, StringLength(125)]
        public string Name { get; set; }

        public List<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

    }
}
