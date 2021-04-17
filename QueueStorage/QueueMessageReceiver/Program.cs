using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace QueueMessageReceiver
{
    class Program
    {
        const string StorageAccountConnectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=oscarlearning2021;AccountKey=Lw+cAi71GYQS6TzHhPN6fWN6d8a3pPSGWQ57GukZ2KRZc+OcIFq81EPoBPOmSuuUk8bf/ekjcMYNgxF4PwVoaQ==";

        const string QueueName = "testqueue";

        static void Main(string[] args)
        {
            Console.WriteLine("Application running...");
            GetQueueMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine("Application finished!!");
        }

        static async Task GetQueueMessageAsync()
        {
            QueueClient queueClient = new QueueClient(StorageAccountConnectionString, QueueName);

            QueueMessage[] retrievedMessage = await queueClient.ReceiveMessagesAsync();
            Console.WriteLine($"Retrieved message with content '{retrievedMessage[0].MessageText}'");

            await queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
            Console.WriteLine($"Deleted message: '{retrievedMessage[0].MessageText}'");

            await queueClient.DeleteAsync();
            Console.WriteLine($"Deleted queue: '{queueClient.Name}'");
        }
    }
}
