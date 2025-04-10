using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using APP.Hospital.Features.Doctors;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Hospital.Features.Branches
{
    public class BranchQueryRequest : Request, IRequest<IQueryable<BranchQueryResponse>>
    {
    }

    public class BranchQueryResponse : QueryResponse
    {
        public string Name { get; set; }

        [JsonIgnore]
        public List<DoctorQueryResponse> Doctors { get; set; }
    }

    public class BranchQueryHandler : HospitalDbHandler, IRequestHandler<BranchQueryRequest, IQueryable<BranchQueryResponse>>
    {
        public BranchQueryHandler(HospitalDb db) : base(db)
        {
        }

        public Task<IQueryable<BranchQueryResponse>> Handle(BranchQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Branches
                .Include(b => b.Doctors)
                .OrderBy(b => b.Name)
                .Select(b => new BranchQueryResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    Doctors = b.Doctors.Select(d => new DoctorQueryResponse
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Surname = d.Surname,
                        FullName = d.Name + " " + d.Surname,
                        BranchId = d.BranchId
                    }).ToList()
                });

            return Task.FromResult(query);
        }
    }
}
