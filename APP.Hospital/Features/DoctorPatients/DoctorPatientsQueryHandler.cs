using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using APP.Hospital.Features.Doctors;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Hospital.Features.DoctorPatients
{
    public class DoctorPatientQueryRequest : Request, IRequest<IQueryable<DoctorPatientQueryResponse>>
    {
    }

    public class DoctorPatientQueryResponse : QueryResponse
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public DoctorQueryResponse Doctor { get; set; }
        public PatientQueryResponse Patient { get; set; }
    }

    public class DoctorPatientQueryHandler : HospitalDbHandler, IRequestHandler<DoctorPatientQueryRequest, IQueryable<DoctorPatientQueryResponse>>
    {
        public DoctorPatientQueryHandler(HospitalDb db) : base(db)
        {
        }

        public Task<IQueryable<DoctorPatientQueryResponse>> Handle(DoctorPatientQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.DoctorPatients
                .Include(dp => dp.Doctor)
                .Include(dp => dp.Patient)
                .OrderBy(dp => dp.DoctorId)
                .Select(dp => new DoctorPatientQueryResponse()
                {
                    Id = dp.Id,
                    DoctorId = dp.DoctorId,
                    PatientId = dp.PatientId,
                    Doctor = new DoctorQueryResponse()
                    {
                        Id = dp.Doctor.Id,
                        Name = dp.Doctor.Name,
                        Surname = dp.Doctor.Surname,
                        FullName = dp.Doctor.Name + " " + dp.Doctor.Surname,
                        BranchId = dp.Doctor.BranchId
                    },
                    Patient = new PatientQueryResponse()
                    {
                        Id = dp.Patient.Id,
                        Name = dp.Patient.Name,
                        Surname = dp.Patient.Surname
                    }
                });

            return Task.FromResult(query);
        }
    }
}
