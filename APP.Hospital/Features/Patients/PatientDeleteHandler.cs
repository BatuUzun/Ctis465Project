using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Hospital.Features.Patients
{
    public class PatientDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class PatientDeleteHandler : HospitalDbHandler, IRequestHandler<PatientDeleteRequest, CommandResponse>
    {
        public PatientDeleteHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(PatientDeleteRequest request, CancellationToken cancellationToken)
        {
            var patient = _db.Patients.Include(p => p.DoctorPatients).SingleOrDefault(p => p.Id == request.Id);
            if (patient is null)
                return Error("Patient not found!");
            _db.DoctorPatients.RemoveRange(patient.DoctorPatients);
            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Patient deleted successfully", patient.Id);
        }
    }
}
