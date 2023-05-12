using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Email.MessageProcessor
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([ServiceBusTrigger("outbox", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message, ILogger log)
        {
            //If getting an error for ServiceBusReceivedMessage - Update Microsoft.Azure.WebJobs.Extensions.ServiceBus to the latest version (5.10)
            var readSourceFromMessageProperty = message.ApplicationProperties["Source"].ToString();

            //Get Documents from blob storage
            //SEND SMTP message
            
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {message}");
        }
    }
}