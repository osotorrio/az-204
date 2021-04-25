using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;

namespace OffsetEventReceiver
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

            string _partition = (await client.GetPartitionIdsAsync()).First();

            var cancellation = new CancellationToken();

            EventPosition _position = EventPosition.FromSequenceNumber(40);

            Console.WriteLine("Getting events from a certain position from a particular partition");

            await foreach (PartitionEvent _recent_event in client.ReadEventsFromPartitionAsync(_partition, _position, cancellation))
            {
                EventData event_data = _recent_event.Data;

                Console.WriteLine(Encoding.UTF8.GetString(event_data.Body.ToArray()));
                Console.WriteLine($"Sequence Number : {event_data.SequenceNumber}");
            }
        }
    }
}
