using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace QueueMessageSender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://servicebus-namespace-2021.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LppqONsxkRDn/MnJGUrTEV6Ijz3yGkUIZh1ykqCI9MQ=";
        const string QueueName = "queue-name";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Messages queue...");
            SendSalesMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendSalesMessageAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Peopiot Gritols",
                Age = 43
            };

            var userStr = JsonConvert.SerializeObject(user);

            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(userStr));

                message.CorrelationId = Guid.NewGuid().ToString();
                message.UserProperties.Add("Id", user.Id);
                message.UserProperties.Add("Name", user.Name);
                message.UserProperties.Add("Age", user.Age);

                Console.WriteLine($"Sending message: {userStr}");
                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            await queueClient.CloseAsync();
        }
    }

    class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}