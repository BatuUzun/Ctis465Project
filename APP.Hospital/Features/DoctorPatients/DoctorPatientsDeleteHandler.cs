using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Hospital.Features.DoctorPatients
{
    public class DoctorPatientDeleteRequest : Request, IRequest<CommandResponse>
    {
        public int Id { get; set; }
    }

    public class DoctorPatientDeleteHandler : HospitalDbHandler, IRequestHandler<DoctorPatientDeleteRequest, CommandResponse>
    {
        public DoctorPatientDeleteHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorPatientDeleteRequest request, CancellationToken cancellationToken)
        {
            var doctorPatient = _db.DoctorPatients.SingleOrDefault(dp => dp.Id == request.Id);
            if (doctorPatient is null)
                return Error("DoctorPatient not found!");

            _db.DoctorPatients.Remove(doctorPatient);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("DoctorPatient deleted successfully", doctorPatient.Id);
        }
    }
}
