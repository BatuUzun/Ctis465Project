using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using APP.Users.Domain;
using APP.Users.Features.Users;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Roles
{
    public class RoleQueryRequest : Request, IRequest<IQueryable<RoleQueryResponse>>
    {
    }

    public class RoleQueryResponse : QueryResponse
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<UserQueryResponse> Users { get; set; }
    }

    public class RoleQueryHandler : UsersDbHandler, IRequestHandler<RoleQueryRequest, IQueryable<RoleQueryResponse>>
    {
        public RoleQueryHandler(UsersDb db) : base(db)
        {
        }

        public Task<IQueryable<RoleQueryResponse>> Handle(RoleQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Roles.Include(r => r.Users)
                .OrderBy(r => r.Name).Select(r => new RoleQueryResponse()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Users = r.Users.Select(u => new UserQueryResponse()
                    {
                        FullName = u.Name + " " + u.Surname,
                        Id = u.Id,
                        IsActive = u.IsActive,
                        IsActiveF = u.IsActive ? "Active" : "Inactive",
                        Name = u.Name,
                        Password = u.Password,
                        Surname = u.Surname,
                        UserName = u.UserName
                    }).ToList()
                });
            return Task.FromResult(query);
        }
    }
}
