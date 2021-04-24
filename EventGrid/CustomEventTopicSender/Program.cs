using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomEventTopicSender
{
    class Program
    {
        static string _topicEndpoint = "https://custom-event-grid-topic-2021.westeurope-1.eventgrid.azure.net/api/events";
        static string _topicKey = "rNq+MIfc42aNMdTuxJQaKf9BKHDJ5MigqkhRURQ0IKM=";
        static async Task Main(string[] args)
        {
            // Data to be sent
            GridEvent gridEvent = new GridEvent
            {
                Id = Guid.NewGuid(),
                Data = $"Hello there at {DateTime.UtcNow}",
                EventTime = DateTime.UtcNow,
                Subject = "Some test subject",
                EventType = $"Some test event type"
            };

            var events = new List<GridEvent>();
            events.Add(gridEvent);

            string jsonData = JsonConvert.SerializeObject(events);

            // Http request
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("aeg-sas-key", _topicKey);

            HttpRequestMessage _request = new HttpRequestMessage(HttpMethod.Post, _topicEndpoint)
            {
                Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _client.SendAsync(_request);

            Console.WriteLine("Event Sent");
            Console.WriteLine(response.Content.ReadAsStringAsync().Result.ToString());
        }
    }
}
