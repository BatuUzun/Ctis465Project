using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Skills
{
    public class SkillDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class SkillDeleteHandler : UsersDbHandler, IRequestHandler<SkillDeleteRequest, CommandResponse>
    {
        public SkillDeleteHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(SkillDeleteRequest request, CancellationToken cancellationToken)
        {
            var skill = _db.Skills.Include(s => s.UserSkills).SingleOrDefault(s => s.Id == request.Id);
            if (skill is null)
                return Error("Skill not found!");
            _db.UserSkills.RemoveRange(skill.UserSkills);
            _db.Skills.Remove(skill);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Skill deleted successfully", skill.Id);
        }
    }
}
