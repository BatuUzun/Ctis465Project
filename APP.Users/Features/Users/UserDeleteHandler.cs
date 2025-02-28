using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Users
{
    public class UserDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class UserDeleteHandler : UsersDbHandler, IRequestHandler<UserDeleteRequest, CommandResponse>
    {
        public UserDeleteHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
        {
            var user = _db.Users.Include(u => u.UserSkills).SingleOrDefault(u => u.Id == request.Id);
            if (user is null)
                return Error("User not found!");
            _db.UserSkills.RemoveRange(user.UserSkills);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("User deleted successfully", user.Id);
        }
    }
}
