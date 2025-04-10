using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Hospital.Features.Doctors
{
    public class DoctorQueryRequest : Request, IRequest<IQueryable<DoctorQueryResponse>>
    {
    }

    public class DoctorQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public int BranchId { get; set; }
        public string Branch { get; set; }
        public List<int> PatientIds { get; set; }
        public List<PatientQueryResponse> Patients { get; set; }
    }

    public class PatientQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class DoctorQueryHandler : HospitalDbHandler, IRequestHandler<DoctorQueryRequest, IQueryable<DoctorQueryResponse>>
    {
        public DoctorQueryHandler(HospitalDb db) : base(db)
        {
        }

        public Task<IQueryable<DoctorQueryResponse>> Handle(DoctorQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Doctors
                .Include(d => d.Branch)
                .Include(d => d.DoctorPatients)
                    .ThenInclude(dp => dp.Patient)
                .OrderBy(d => d.Name)
                .Select(d => new DoctorQueryResponse()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Surname = d.Surname,
                    FullName = d.Name + " " + d.Surname,
                    Branch = d.Branch.Name,
                    BranchId = d.BranchId,
                    PatientIds = d.DoctorPatients.Select(dp => dp.PatientId).ToList(),
                    Patients = d.DoctorPatients.Select(dp => new PatientQueryResponse
                    {
                        Id = dp.Patient.Id,
                        Name = dp.Patient.Name,
                        Surname = dp.Patient.Surname
                    }).ToList()
                });

            return Task.FromResult(query);
        }
    }
}
