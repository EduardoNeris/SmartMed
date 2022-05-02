using System.Threading.Tasks;
using System.Collections.Generic;
using MedicationWebApi.Models;

namespace MedicationWebApi.DataAccess
{
    public interface IRepository
    {
        Task<IEnumerable<Medication>> GetMedication();
        Task<Medication> GetMedicationById(int id);
        Task<Medication> AddMedication(Medication medication);
        Task<Medication> DeleteMedication(Medication medication);
    }
}