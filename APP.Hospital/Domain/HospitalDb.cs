using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace APP.Hospital.Domain
{
    public class HospitalDb : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }

        public HospitalDb(DbContextOptions options) : base(options)
        {
        }
    }

    public class HospitalDbFactory : IDesignTimeDbContextFactory<HospitalDb>
    {
        public HospitalDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HospitalDb>();

            optionsBuilder.UseSqlServer("server=127.0.0.1,1433;database=PMSHospitalDB;user id=sa;password=Cagil123!;trustservercertificate=true;");

            return new HospitalDb(optionsBuilder.Options);
        }
    }
}
