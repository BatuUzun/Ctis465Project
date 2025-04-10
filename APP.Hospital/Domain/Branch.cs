using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.APP.Domain;

namespace APP.Hospital.Domain
{
    public class Branch : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
