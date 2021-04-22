using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace TopicMessageSender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://servicebus-learn-paths-salesteamapp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zBq34zUmK9m/jW/n8Zf18c2wOrzGghiQLpZmKpwH+no=";
        const string TopicName = "salesperformancemessages";
        static ITopicClient topicClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Sending a message to the Sales Performance topic...");
            SendPerformanceMessageAsync().GetAwaiter().GetResult();
            Console.WriteLine("Message was sent successfully.");
        }

        static async Task SendPerformanceMessageAsync()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            try
            {
                string messageBody = $"Total sales for Brazil in August: $13m.";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                Console.WriteLine($"Sending message: {messageBody}");
                await topicClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            await topicClient.CloseAsync();
        }
    }
}