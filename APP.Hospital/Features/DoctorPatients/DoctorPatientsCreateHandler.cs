using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Hospital.Features.DoctorPatients
{
    public class DoctorPatientCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }
    }

    public class DoctorPatientCreateHandler : HospitalDbHandler, IRequestHandler<DoctorPatientCreateRequest, CommandResponse>
    {
        public DoctorPatientCreateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorPatientCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.DoctorPatients.Any(dp => dp.DoctorId == request.DoctorId && dp.PatientId == request.PatientId))
                return Error("DoctorPatient with the same DoctorId and PatientId already exists!");

            var doctorPatient = new DoctorPatient()
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId
            };

            _db.DoctorPatients.Add(doctorPatient);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("DoctorPatient created successfully.", doctorPatient.Id);
        }
    }
}
