using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace MessageSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string text;

                do
                {
                    Console.WriteLine("Type the message to be sent:");
                    text = Console.ReadLine();
                    await Execute(text);
                    Console.WriteLine("Message sent successfully");
                }
                while (text.ToLower() != "exit");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        static async Task Execute(string text)
        {
            var message = $"Message: {text}. Date: {DateTime.UtcNow}";

            var connectionString = "Endpoint=sb://servicebus-learn-paths-salesteamapp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zBq34zUmK9m/jW/n8Zf18c2wOrzGghiQLpZmKpwH+no=";

            var qClient = new QueueClient(connectionString, "salesteamapp");

            var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));

            await qClient.SendAsync(encodedMessage);
        }
    }
}
