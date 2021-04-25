using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace OnePartitionEventSender
{
    class Program
    {
        private static string connstring = "Endpoint=sb://appname.servicebus.windows.net/;SharedAccessKeyName=demo;SharedAccessKey=Dz60v7IXFFmFGWVMGSfmONyouCHa0S3/IvQINDZ3rAw=;EntityPath=apphub";
        private static string hubname = "apphub";

        static String[] courses = new String[]
        {
            "AZ-900","AZ-400","AZ-300","AZ-301","AZ-204"
        };

        static void Main(string[] args)
        {
            SendData().Wait();
        }

        private static async Task SendData()
        {

            EventHubProducerClient client = new EventHubProducerClient(connstring, hubname);

            string _partition = (await client.GetPartitionIdsAsync()).First();

            var _options = new CreateBatchOptions
            {
                PartitionId = _partition
            };

            EventDataBatch batch_obj = await client.CreateBatchAsync(_options);
            Random _rnd = new Random();

            for (int i = 1; i <= 10; i++)
            {
                Order obj = new Order(i, courses[_rnd.Next(0, 4)], _rnd.Next(1, 100));
                batch_obj.TryAdd(new EventData(Encoding.UTF8.GetBytes(obj.ToString())));

            }
            await client.SendAsync(batch_obj);

            Console.WriteLine("Sent all Orders");

        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public Order(int id, string category, int quantity)
        {
            Id = id;
            Category = category;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Id : {Id}, Category : {Category} , Quantity {Quantity}";
        }
    }
}
