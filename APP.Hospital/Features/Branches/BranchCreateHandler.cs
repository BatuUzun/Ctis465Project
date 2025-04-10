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
    public class BranchCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public class BranchCreateHandler : HospitalDbHandler, IRequestHandler<BranchCreateRequest, CommandResponse>
    {
        public BranchCreateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Branches.Any(b => b.Name == request.Name))
                return Error("Branch with the same name exists!");

            var branch = new Branch
            {
                Name = request.Name
            };

            _db.Branches.Add(branch);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Branch created successfully.", branch.Id);
        }
    }
}
