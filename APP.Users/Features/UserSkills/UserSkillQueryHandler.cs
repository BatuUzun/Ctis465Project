using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using APP.Users.Domain;
using APP.Users.Features.Skills;
using APP.Users.Features.Users;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.UserSkills
{
    public class UserSkillQueryRequest : Request, IRequest<IQueryable<UserSkillQueryResponse>>
    {
    }

    public class UserSkillQueryResponse : QueryResponse
    {
        public int UserId { get; set; }
        public int SkillId { get; set; }

        public UserQueryResponse User { get; set; }
        public SkillQueryResponse Skill { get; set; }
    }

    public class UserSkillQueryHandler : UsersDbHandler, IRequestHandler<UserSkillQueryRequest, IQueryable<UserSkillQueryResponse>>
    {
        public UserSkillQueryHandler(UsersDb db) : base(db)
        {
        }

        public Task<IQueryable<UserSkillQueryResponse>> Handle(UserSkillQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.UserSkills
                .Include(us => us.User)
                .Include(us => us.Skill)
                .OrderBy(us => us.UserId)
                .Select(us => new UserSkillQueryResponse()
                {
                    Id = us.Id,
                    UserId = us.UserId,
                    SkillId = us.SkillId,
                    User = new UserQueryResponse()
                    {
                        Id = us.User.Id,
                        FullName = us.User.Name + " " + us.User.Surname,
                        UserName = us.User.UserName,
                        IsActive = us.User.IsActive,
                        IsActiveF = us.User.IsActive ? "Active" : "Inactive",
                        Name = us.User.Name,
                        Surname = us.User.Surname,
                        Password = us.User.Password
                    },
                    Skill = new SkillQueryResponse()
                    {
                        Id = us.Skill.Id,
                        Name = us.Skill.Name
                    }
                });

            return Task.FromResult(query);
        }
    }
}
