using System;
using Xunit;
using MedicationWebApi.DataAccess;
using MedicationWebApi.DTO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using FluentAssertions;

namespace MedicationTests
{
    public class UnitTest1 : IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public UnitTest1()
        {
            SetUpClient();
        }

        public void Dispose() { }

        internal async Task SeedData()
        {
            // Create entry with id 1 
            var createForm0 = GenerateCreateForm("Medication1", DateTime.Now, 1576436425);
            var response0 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));

            // Create entry with id 2 
            var createForm1 = GenerateCreateForm("Medication2", DateTime.Now, 1576436425);
            var response1 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));

            // Create entry with id 3 
            var createForm2 = GenerateCreateForm("Medication3", DateTime.Now, 1576436606);
            var response2 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm2), Encoding.UTF8, "application/json"));

            // Create entry with id 4 
            var createForm3 = GenerateCreateForm("Medication4", DateTime.Now, 1576435425);
            var response3 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm3), Encoding.UTF8, "application/json"));

            // Create entry with id 5
            var createForm4 = GenerateCreateForm("Medication5", DateTime.Now, 1576535425);
            var response4 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm4), Encoding.UTF8, "application/json"));
        }

        // TEST - GetMedication
        // TEST DESCRIPTION - It finds all the medications
        [Fact]
        public async Task TestCase1()
        {
            await SeedData();

            // Get All medications 
            var response0 = await Client.GetAsync("/api/medication");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("[{\"id\":1,\"name\":\"Medication1\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1576436425\"},{\"id\":2,\"name\":\"Medication2\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1576436425\"},{\"id\":3,\"name\":\"Medication3\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1576436606\"},{\"id\":4,\"name\":\"Medication4\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1576435425\"},{\"id\":5,\"name\":\"Medication5\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1576535425\"}]");
            realData0.Should().BeEquivalentTo(expectedData0);
        }

        //// TEST NAME - CreateMedication
        //// TEST DESCRIPTION - A new medication should be created
        [Fact]
        public async Task TestCase2()
        {
            await SeedData();

            // Create entry with id 6
            var createForm0 = GenerateCreateForm("Medication6", DateTime.Now, 1577535425);
            var response0 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));
            response0.StatusCode.Should().BeEquivalentTo(201);

            var realData0 = JsonConvert.DeserializeObject(response0.Content.ReadAsStringAsync().Result);
            var expectedData0 = JsonConvert.DeserializeObject("{\"id\":6,\"name\":\"Medication6\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1577535425\"}");
            realData0.Should().BeEquivalentTo(expectedData0);

            // Create entry with id 7
            var createForm1 = GenerateCreateForm("Medication7", DateTime.Now, 1573535425);
            var response1 = await Client.PostAsync("/api/medication", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));
            response1.StatusCode.Should().BeEquivalentTo(201);

            var realData1 = JsonConvert.DeserializeObject(response1.Content.ReadAsStringAsync().Result);
            var expectedData1 = JsonConvert.DeserializeObject("{\"id\":7,\"name\":\"Medication7\",\"creationDate\":\"2022-05-02T00:00:00.000Z\",\"quantity\":\"1573535425\"}");
            realData1.Should().BeEquivalentTo(expectedData1);
        }

        // TEST NAME - deleteMedication
        // TEST DESCRIPTION - Delete an medication by id
        [Fact]
        public async Task TestCase3()
        {
            await SeedData();

            // Return with 204 if medication is deleted
            var response0 = await Client.DeleteAsync("/api/medication/3");
            response0.StatusCode.Should().Be(204);

            // Check if the medication does not exist
            var response2 = await Client.GetAsync("api/medication/query?id=3");
            response2.StatusCode.Should().BeEquivalentTo(200);
            var realData2 = JsonConvert.DeserializeObject(response2.Content.ReadAsStringAsync().Result);
            realData2.Should().Equals("[]");
        }

        private CreateForm GenerateCreateForm(string name, DateTime creationDate, int quantity)
        {
            return new CreateForm()
            {
                Name = name,
                CreationDate = creationDate,
                Quantity = quantity
            };
        }

        private void SetUpClient()
        {

            var builder = new WebHostBuilder()
                .UseStartup<MedicationWebApi.Startup>()
                .ConfigureServices(services =>
                {
                    var context = new MedicationContext(new DbContextOptionsBuilder<MedicationContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(MedicationContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}
