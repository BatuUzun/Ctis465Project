using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Hospital.Features.Branches
{
    public class BranchUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public class BranchUpdateHandler : HospitalDbHandler, IRequestHandler<BranchUpdateRequest, CommandResponse>
    {
        public BranchUpdateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Branches.Any(b => b.Id != request.Id && b.Name == request.Name))
                return Error("Branch with the same name exists!");

            var branch = _db.Branches.Find(request.Id);
            if (branch is null)
                return Error("Branch not found!");

            branch.Name = request.Name;
            _db.Branches.Update(branch);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Branch updated successfully.", branch.Id);
        }
    }
}
