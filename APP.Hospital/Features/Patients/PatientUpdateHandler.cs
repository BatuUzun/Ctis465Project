using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Hospital.Features.Patients
{
    public class PatientUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Surname { get; set; }

        [Required]
        public bool IsFemale { get; set; }

        public DateTime BirthDate { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }
    }

    public class PatientUpdateHandler : HospitalDbHandler, IRequestHandler<PatientUpdateRequest, CommandResponse>
    {
        public PatientUpdateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(PatientUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Patients.Any(p => p.Id != request.Id && p.Name == request.Name && p.Surname == request.Surname && p.BirthDate == request.BirthDate))
                return Error("Patient with the same name, surname and birth date exists!");

            var patient = _db.Patients.Find(request.Id);
            if (patient is null)
                return Error("Patient not found!");

            patient.Name = request.Name;
            patient.Surname = request.Surname;
            patient.IsFemale = request.IsFemale;
            patient.BirthDate = request.BirthDate;
            patient.Height = request.Height;
            patient.Weight = request.Weight;

            _db.Patients.Update(patient);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Patient updated successfully.", patient.Id);
        }
    }
}
