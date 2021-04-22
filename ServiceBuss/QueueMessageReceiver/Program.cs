using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace QueueMessageReceiver
{
    class Program
    {

        const string ServiceBusConnectionString = "Endpoint=sb://servicebus-namespace-2021.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LppqONsxkRDn/MnJGUrTEV6Ijz3yGkUIZh1ykqCI9MQ=";
        const string QueueName = "queue-name";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            ReceiveSalesMessageAsync().GetAwaiter().GetResult();
        }

        static async Task ReceiveSalesMessageAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            RegisterMessageHandler();

            Console.Read();

            await queueClient.CloseAsync();
        }

        static void RegisterMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false

            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var userStr = Encoding.UTF8.GetString(message.Body);
            var user = JsonConvert.DeserializeObject<User>(userStr);

            Console.WriteLine(userStr);
            Console.WriteLine($"Id: {user.Id}");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Age: {user.Age}");
            Console.WriteLine($"MessageId: {message.MessageId}");
            Console.WriteLine($"CorrelationId: {message.CorrelationId}");
            Console.WriteLine($"SequenceNumber: {message.SystemProperties.SequenceNumber}");
            Console.WriteLine($"Custom Id: {message.UserProperties["Id"]}");
            Console.WriteLine($"Custom Name: {message.UserProperties["Name"]}");
            Console.WriteLine($"Custom Age: {message.UserProperties["Age"]}");

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }

    class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}