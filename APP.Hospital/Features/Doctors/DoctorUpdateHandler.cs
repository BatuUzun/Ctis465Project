using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Hospital.Features.Doctors
{
    public class DoctorUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }

        public int BranchId { get; set; }

        public List<int> PatientIds { get; set; }
    }

    public class DoctorUpdateHandler : HospitalDbHandler, IRequestHandler<DoctorUpdateRequest, CommandResponse>
    {
        public DoctorUpdateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(DoctorUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Doctors.Any(d => d.Id != request.Id && (d.Name == request.Name && d.Surname == request.Surname)))
                return Error("Doctor with the same full name exists!");

            var doctor = _db.Doctors.Include(d => d.DoctorPatients).SingleOrDefault(d => d.Id == request.Id);
            if (doctor is null)
                return Error("Doctor not found!");

            _db.DoctorPatients.RemoveRange(doctor.DoctorPatients);

            doctor.Name = request.Name;
            doctor.Surname = request.Surname;
            doctor.BranchId = request.BranchId;
            doctor.DoctorPatients = request.PatientIds?.Select(pid => new DoctorPatient
            {
                PatientId = pid
            }).ToList() ?? new List<DoctorPatient>();

            _db.Doctors.Update(doctor);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Doctor updated successfully.", doctor.Id);
        }
    }
}
