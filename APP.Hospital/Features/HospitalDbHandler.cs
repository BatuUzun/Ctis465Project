using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Hospital.Domain;
using CORE.APP.Features;

namespace APP.Hospital.Features
{
    public abstract class HospitalDbHandler : Handler
    {
        protected readonly HospitalDb _db;

        protected HospitalDbHandler(HospitalDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
    }
}
