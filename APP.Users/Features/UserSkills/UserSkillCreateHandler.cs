using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Users.Features.UserSkills
{
    public class UserSkillCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int SkillId { get; set; }
    }

    public class UserSkillCreateHandler : UsersDbHandler, IRequestHandler<UserSkillCreateRequest, CommandResponse>
    {
        public UserSkillCreateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserSkillCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.UserSkills.Any(us => us.UserId == request.UserId && us.SkillId == request.SkillId))
                return Error("UserSkill with the same UserId and SkillId already exists!");

            var userSkill = new UserSkill()
            {
                UserId = request.UserId,
                SkillId = request.SkillId
            };

            _db.UserSkills.Add(userSkill);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("UserSkill created successfully.", userSkill.Id);
        }
    }
}
