using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Hospital.Features.Patients
{
    public class PatientQueryRequest : Request, IRequest<IQueryable<PatientQueryResponse>>
    {
    }

    public class PatientQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class PatientQueryHandler : HospitalDbHandler, IRequestHandler<PatientQueryRequest, IQueryable<PatientQueryResponse>>
    {
        public PatientQueryHandler(HospitalDb db) : base(db)
        {
        }

        public Task<IQueryable<PatientQueryResponse>> Handle(PatientQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Patients
                .OrderBy(p => p.Name)
                .Select(p => new PatientQueryResponse()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Surname = p.Surname
                });

            return Task.FromResult(query);
        }
    }
}
