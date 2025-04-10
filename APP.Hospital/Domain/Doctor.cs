using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Domain;

namespace APP.Hospital.Domain
{
    public class Doctor : Entity
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public List<DoctorPatient> DoctorPatients { get; set; } = new List<DoctorPatient>();
    }
}
