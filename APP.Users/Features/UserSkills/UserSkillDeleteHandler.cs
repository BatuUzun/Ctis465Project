using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.UserSkills
{
    public class UserSkillDeleteRequest : Request, IRequest<CommandResponse>
    {
        public int Id { get; set; }
    }

    public class UserSkillDeleteHandler : UsersDbHandler, IRequestHandler<UserSkillDeleteRequest, CommandResponse>
    {
        public UserSkillDeleteHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserSkillDeleteRequest request, CancellationToken cancellationToken)
        {
            var userSkill = _db.UserSkills.SingleOrDefault(us => us.Id == request.Id);
            if (userSkill is null)
                return Error("UserSkill not found!");

            _db.UserSkills.Remove(userSkill);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("UserSkill deleted successfully", userSkill.Id);
        }
    }
}
