using System;
using System.ComponentModel.DataAnnotations;

namespace MedicationWebApi.Models
{
    public class Medication
    {
        /// <summary>
        /// id of the medication
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name of the medication
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Creation date of the medication
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Quantity of the medication
        /// </summary>
        public int Quantity { get; set; }
    }
}
