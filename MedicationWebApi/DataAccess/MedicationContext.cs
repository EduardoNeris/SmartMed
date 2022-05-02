using Microsoft.EntityFrameworkCore;
using MedicationWebApi.Models;

namespace MedicationWebApi.DataAccess
{
    public class MedicationContext : DbContext
    {
        public MedicationContext(DbContextOptions<MedicationContext> options)
            : base(options)
        { }

        public DbSet<Medication> Medication { get; set; }
    }
}
