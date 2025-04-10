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
    public class DoctorPatientUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }
    }

    public class DoctorPatientUpdateHandler : HospitalDbHandler, IRequestHandler<DoctorPatientUpdateRequest, CommandResponse>
    {
        public DoctorPatientUpdateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorPatientUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.DoctorPatients.Any(dp => dp.Id != request.Id && dp.DoctorId == request.DoctorId && dp.PatientId == request.PatientId))
                return Error("Another DoctorPatient entry with the same DoctorId and PatientId already exists!");

            var doctorPatient = _db.DoctorPatients.Find(request.Id);
            if (doctorPatient is null)
                return Error("DoctorPatient not found!");

            doctorPatient.DoctorId = request.DoctorId;
            doctorPatient.PatientId = request.PatientId;

            _db.DoctorPatients.Update(doctorPatient);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("DoctorPatient updated successfully.", doctorPatient.Id);
        }
    }
}
