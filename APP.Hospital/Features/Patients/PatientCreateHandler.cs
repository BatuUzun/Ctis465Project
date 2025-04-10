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
    public class PatientCreateRequest : Request, IRequest<CommandResponse>
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

    public class PatientCreateHandler : HospitalDbHandler, IRequestHandler<PatientCreateRequest, CommandResponse>
    {
        public PatientCreateHandler(HospitalDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(PatientCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Patients.Any(p => p.Name == request.Name && p.Surname == request.Surname && p.BirthDate == request.BirthDate))
                return Error("Patient with the same name, surname and birth date exists!");

            var patient = new Patient()
            {
                Name = request.Name,
                Surname = request.Surname,
                IsFemale = request.IsFemale,
                BirthDate = request.BirthDate,
                Height = request.Height,
                Weight = request.Weight
            };

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Patient created successfully.", patient.Id);
        }
    }
}
