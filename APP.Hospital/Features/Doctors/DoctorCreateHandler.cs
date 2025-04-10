using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Hospital.Features.Doctors
{
    public class DoctorCreateRequest : Request, IRequest<CommandResponse>
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

    public class DoctorCreateHandler : HospitalDbHandler, IRequestHandler<DoctorCreateRequest, CommandResponse>
    {
        public DoctorCreateHandler(HospitalDb db) : base(db)
        {
        }
        
        public async Task<CommandResponse> Handle(DoctorCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Doctors.Any(d => d.Name == request.Name && d.Surname == request.Surname))
                return Error("Doctor with the same full name exists!");

            var doctor = new Doctor
            {
                Name = request.Name,
                Surname = request.Surname,
                BranchId = request.BranchId,
                DoctorPatients = request.PatientIds?.Select(pid => new DoctorPatient
                {
                    PatientId = pid
                }).ToList() ?? new List<DoctorPatient>()
            };

            _db.Doctors.Add(doctor);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Doctor created successfully.", doctor.Id);
        }
    }
}
