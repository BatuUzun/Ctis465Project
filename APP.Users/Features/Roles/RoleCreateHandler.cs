using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Users.Features.Roles
{
    public class RoleCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }

    public class RoleCreateHandler : UsersDbHandler, IRequestHandler<RoleCreateRequest, CommandResponse>
    {
        public RoleCreateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Roles.Any(r => r.Name == request.Name))
                return Error("Role with the same name exists!");
            var role = new Role()
            {
                Name = request.Name
            };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Role created successfully.", role.Id);
        }
    }
}
