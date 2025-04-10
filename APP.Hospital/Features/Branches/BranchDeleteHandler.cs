using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Hospital.Features.Branches
{
    public class BranchDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class BranchDeleteHandler : HospitalDbHandler, IRequestHandler<BranchDeleteRequest, CommandResponse>
    {
        public BranchDeleteHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BranchDeleteRequest request, CancellationToken cancellationToken)
        {
            var branch = _db.Branches.Include(b => b.Doctors).SingleOrDefault(b => b.Id == request.Id);
            if (branch is null)
                return Error("Branch not found!");
            if (branch.Doctors.Count > 0)
                return Error("Branch can't be deleted because it has relational doctors!");
            _db.Branches.Remove(branch);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Branch deleted successfully", branch.Id);
        }
    }
}
