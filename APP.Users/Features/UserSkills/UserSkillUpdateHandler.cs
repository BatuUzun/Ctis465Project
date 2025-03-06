using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Users.Features.UserSkills
{
    public class UserSkillUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SkillId { get; set; }
    }

    public class UserSkillUpdateHandler : UsersDbHandler, IRequestHandler<UserSkillUpdateRequest, CommandResponse>
    {
        public UserSkillUpdateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserSkillUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.UserSkills.Any(us => us.Id != request.Id && us.UserId == request.UserId && us.SkillId == request.SkillId))
                return Error("Another UserSkill entry with the same UserId and SkillId already exists!");

            var userSkill = _db.UserSkills.Find(request.Id);
            if (userSkill is null)
                return Error("UserSkill not found!");

            userSkill.UserId = request.UserId;
            userSkill.SkillId = request.SkillId;

            _db.UserSkills.Update(userSkill);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("UserSkill updated successfully.", userSkill.Id);
        }
    }
}
