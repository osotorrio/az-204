using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using LumenWorks.Framework.IO.Csv;

namespace EventSender
{
    class Program
    {
        private static string connstring = "Endpoint=sb://event-hubs-namespace-2021.servicebus.windows.net/;SharedAccessKeyName=send-policy;SharedAccessKey=TNfRyuirM5oPt/i1oDF+9EuoGaG/EEjjCgvr7m+X1WA=;EntityPath=event-hub-name-2021";
        private static string hubname = "event-hub-name-2021";
        static DataTable dt_table;

        static void Main(string[] args)
        {
            LoadData();
            SendData().Wait();
        }
        private static void LoadData()
        {
            dt_table = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead("QueryResult.csv")), true))
            {
                dt_table.Load(csvReader);
            }
        }

        private static async Task SendData()
        {
            EventHubProducerClient client = new EventHubProducerClient(connstring, hubname);

            foreach (DataRow row in dt_table.Rows)
            {
                ActivityData obj = new ActivityData();
                obj.CorrelationId = row[0].ToString();
                obj.OperationName = row[1].ToString();
                obj.Status = row[2].ToString();
                obj.EventCategory = row[3].ToString();
                obj.Level = row[4].ToString();
                obj.Date = DateTime.Parse(row[5].ToString());
                obj.Subscription = row[6].ToString();
                obj.InitiatedBy = row[7].ToString();
                obj.ResourceType = row[8].ToString();
                obj.ResourceGroup = row[9].ToString();
                obj.Resource = row[10].ToString();
                obj.Id = Guid.NewGuid();

                var batch = await client.CreateBatchAsync();

                batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(obj.ToString())));

                await client.SendAsync(batch);

                Console.WriteLine("Sending Data {0}", obj.Id);
            }
        }
    }
}
