using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;

namespace EventReceiver
{
    class Program
    {
        private static string connstring = "Endpoint=sb://event-hubs-namespace-2021.servicebus.windows.net/;SharedAccessKeyName=receive-policy;SharedAccessKey=poTrOAV0QkjQY5UrIWt5krpI4jVNFDq+2z8aaSxO+jU=;EntityPath=event-hub-name-2021";
        private static string hubname = "event-hub-name-2021";

        static void Main(string[] args)
        {
            GetEvents().Wait();
        }

        static async Task GetEvents()
        {
            EventHubConsumerClient client = new EventHubConsumerClient("$Default", connstring, hubname);

            var cancellation = new CancellationToken();

            Console.WriteLine("Getting the events");

            await foreach (PartitionEvent Allevent in client.ReadEventsAsync(cancellation))
            {
                EventData event_data = Allevent.Data;
                Console.WriteLine(Encoding.UTF8.GetString(event_data.Body.ToArray()));
            }
        }
    }
}
