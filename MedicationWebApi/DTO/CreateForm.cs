using System;
using Newtonsoft.Json;
namespace MedicationWebApi.DTO
{
    public class CreateForm
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
