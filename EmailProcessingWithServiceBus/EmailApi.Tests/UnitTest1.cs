using Email.Shared;
using System.Text;
using System.Text.Json;

namespace EmailApi.Tests
{
    [TestFixture]
    public class Tests
    {
        private HttpClient _client;
        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
        }

        [Test]
        public async Task PostMessage()
        {
            var emailRequest = new EmailRequest
            {
                Body = "Test email body",
                From = "test@email.com",
                To = new List<string> { "Test1@email.com" },
                Attachments = new List<EmailAttachment> { 
                    new EmailAttachment { FileName = "Doc1.json", ContentType="application/json", Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new {data = "Test content"})))},
                    new EmailAttachment { FileName = "Doc2.json", ContentType="application/json", Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new {data = "Test content2"})))}
                }
            };
            _client.BaseAddress = new Uri("http://localhost:7240/api/Function1");

            var json = JsonSerializer.Serialize(emailRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("",content);


            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
        }
    }
}