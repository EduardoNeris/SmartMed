using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicationWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicationWebApi.DataAccess
{
    public class Repository : IRepository
    {
        private readonly MedicationContext _context;

        public Repository(MedicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medication>> GetMedication()
        {
            IQueryable<Medication> medications = _context.Medication;
            return await medications
                .Where(o => o.Quantity > 0)
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        public async Task<Medication> AddMedication(Medication medication)
        {
            await _context.Medication.AddAsync(medication);
            await _context.SaveChangesAsync();

            return medication;
        }

        public async Task<Medication> GetMedicationById(int id)
        {
            IQueryable<Medication> medications = _context.Medication;

            if (id > 0)
            {
                medications = medications.Where(o => o.Id == id);
                return await medications.FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<Medication> DeleteMedication(Medication medication)
        {
            _context.Medication.Remove(medication);
            await _context.SaveChangesAsync();

            return medication;
        }
    }
}
