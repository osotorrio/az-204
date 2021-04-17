using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace QueueMessageSender
{
    class Program
    {
        const string StorageAccountConnectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=oscarlearning2021;AccountKey=Lw+cAi71GYQS6TzHhPN6fWN6d8a3pPSGWQ57GukZ2KRZc+OcIFq81EPoBPOmSuuUk8bf/ekjcMYNgxF4PwVoaQ==";

        const string QueueName = "testqueue";

        static void Main(string[] args)
        {
            Console.WriteLine("Write the message to be sent to the queue:");
            var message = Console.ReadLine();
            CreateQueueMessageAsync(message).GetAwaiter().GetResult();
            Console.WriteLine("Message sent to the queue!!");
        }

        static async Task CreateQueueMessageAsync(string message)
        {
            QueueClient queueClient = new QueueClient(StorageAccountConnectionString, QueueName);

            await queueClient.CreateIfNotExistsAsync();

            if (await queueClient.ExistsAsync())
            {
                Console.WriteLine($"Queue '{queueClient.Name}' created");
            }
            else
            {
                Console.WriteLine($"Queue '{queueClient.Name}' exists");
            }

            await queueClient.SendMessageAsync(message);
        }
    }
}
