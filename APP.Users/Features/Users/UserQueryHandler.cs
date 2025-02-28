using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using APP.Users.Features.Skills;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
    }

    public class UserQueryResponse : QueryResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveF { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string RegistrationDateF { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public List<int> SkillIds { get; set; }
        public List<SkillQueryResponse> Skills { get; set; }
    }

    public class UserQueryHandler : UsersDbHandler, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(UsersDb db) : base(db)
        {
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Users.Include(u => u.Role).Include(u => u.UserSkills).ThenInclude(us => us.Skill)
                .OrderBy(u => u.Name).Select(u => new UserQueryResponse()
                {
                    Id = u.Id,
                    Name = u.Name,
                    FullName = u.Name + " " + u.Surname,
                    IsActive = u.IsActive,
                    IsActiveF = u.IsActive ? "Active" : "Inactive",
                    Password = u.Password,
                    Role = u.Role.Name,
                    Surname = u.Surname,
                    UserName = u.UserName,
                    RegistrationDate = u.RegistrationDate,
                    RegistrationDateF = u.RegistrationDate.HasValue ? u.RegistrationDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                    RoleId = u.RoleId,
                    SkillIds = u.SkillIds,
                    Skills = u.UserSkills.Select(us => new SkillQueryResponse()
                    {
                        Id = us.Skill.Id,
                        Name = us.Skill.Name
                    }).ToList()
                });
            return Task.FromResult(query);
        }
    }
}
