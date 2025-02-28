using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Roles
{
    public class RoleDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class RoleDeleteHandler : UsersDbHandler, IRequestHandler<RoleDeleteRequest, CommandResponse>
    {
        public RoleDeleteHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleDeleteRequest request, CancellationToken cancellationToken)
        {
            var role = _db.Roles.Include(r => r.Users).SingleOrDefault(r => r.Id == request.Id);
            if (role is null)
                return Error("Role not found!");
            if (role.Users.Count > 0)
                return Error("Role can't be deleted becuase it has relational users!");
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Role deleted successfully", role.Id);
        }
    }
}
