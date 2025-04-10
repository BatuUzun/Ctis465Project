using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Domain;

namespace APP.Hospital.Domain
{
    public class Patient : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsFemale { get; set; }
        public DateTime BirthDate { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }

        public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();

        [NotMapped]
        public List<int> DoctorIds
        {
            get => DoctorPatients.Select(dp => dp.DoctorId).ToList();
            set => DoctorPatients = value.Select(id => new DoctorPatient { DoctorId = id }).ToList();
        }
    }
}
