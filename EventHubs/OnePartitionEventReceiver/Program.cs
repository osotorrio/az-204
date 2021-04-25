using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;

namespace OnePartitionEventReceiver
{
    class Program
    {
        private static string connstring = "Endpoint=sb://appname.servicebus.windows.net/;SharedAccessKeyName=demo;SharedAccessKey=Dz60v7IXFFmFGWVMGSfmONyouCHa0S3/IvQINDZ3rAw=;EntityPath=apphub";
        private static string hubname = "apphub";

        static void Main(string[] args)
        {
            GetEvents().Wait();
        }

        static private async Task GetEvents()
        {
            EventHubConsumerClient client = new EventHubConsumerClient("$Default", connstring, hubname);

            var cancellation = new CancellationToken();

            Console.WriteLine("Getting the events");

            await foreach (PartitionEvent Allevent in client.ReadEventsAsync(cancellation))
            {
                EventData event_data = Allevent.Data;

                Console.WriteLine($"Partition Id : {Allevent.Partition.PartitionId}");
                Console.WriteLine(Encoding.UTF8.GetString(event_data.Body.ToArray()));
            }
        }
    }
}