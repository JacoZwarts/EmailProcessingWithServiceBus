using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Threading.Tasks;
using Email.Shared;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Messaging.ServiceBus;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace Email.API
{
    public class Function1
    {
        private readonly IConfiguration _configuration;
        public Function1(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,"post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var test = _configuration["BlobStorageConnectionString"];

            //TODO extract payload from request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var email = JsonSerializer.Deserialize<EmailRequest>(requestBody);

            //TODO extract documents and upload to Blob Storage
            var container = new BlobContainerClient(_configuration["BlobStorageConnectionString"], "attachments");
            await container.CreateIfNotExistsAsync();


            var requestMessaage = new EmailRequestMessage
            {
                Body = email.Body,
                From = email.From,
                To = email.To
            };

            foreach(var attachment in email.Attachments)
            {
                try
                {
                    await container.UploadBlobAsync(attachment.FileName, new BinaryData(attachment.Content));
                }
                catch (Exception)
                {
                    //File already exist 
                }
                requestMessaage.Attachments.Add(attachment.FileName);

            }

            var message = new ServiceBusMessage
            {
                MessageId = "TestEmail1",
                Body = new BinaryData(requestMessaage),
                ApplicationProperties =
                {
                    ["Source"] = "TestClient1"
                }
            };
            var client = new ServiceBusClient(_configuration["ServiceBusConnectionString"]);
            ServiceBusSender sender = client.CreateSender("outbox");
            await sender.SendMessageAsync(message);


            return new OkObjectResult("Request Processed");
        }
    }
}