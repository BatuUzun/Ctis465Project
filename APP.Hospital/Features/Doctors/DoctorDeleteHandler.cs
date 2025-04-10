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
    public class DoctorDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class DoctorDeleteHandler : HospitalDbHandler, IRequestHandler<DoctorDeleteRequest, CommandResponse>
    {
        public DoctorDeleteHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorDeleteRequest request, CancellationToken cancellationToken)
        {
            var doctor = _db.Doctors.Include(d => d.DoctorPatients).SingleOrDefault(d => d.Id == request.Id);
            if (doctor is null)
                return Error("Doctor not found!");
            _db.DoctorPatients.RemoveRange(doctor.DoctorPatients);
            _db.Doctors.Remove(doctor);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Doctor deleted successfully", doctor.Id);
        }
    }
}
