using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;

namespace ProcessorEventReceiver
{
    class Program
    {
        private static string _bus_connection = "Endpoint=sb://appname.servicebus.windows.net/;SharedAccessKeyName=demo;SharedAccessKey=Dz60v7IXFFmFGWVMGSfmONyouCHa0S3/IvQINDZ3rAw=;EntityPath=apphub";
        private static string _hubname = "apphub";
        private static string _storage_account = "DefaultEndpointsProtocol=https;AccountName=demostore3000;AccountKey=uTR9XQL25G/uxoimMo8xusantfPwhrdNXcssNA0g7Od5pM4Uzxl9C+i0U1iIRCM3JucXq9/F34NceJ85yHNdiw==;EndpointSuffix=core.windows.net";
        private static string _container = "processor";
        static async Task Main(string[] args)
        {
            BlobContainerClient _blob_client = new BlobContainerClient(_storage_account, _container);

            EventProcessorClient _event_client = new EventProcessorClient(_blob_client, "$Default", _bus_connection, _hubname);

            _event_client.ProcessEventAsync += Process_Message;
            _event_client.ProcessErrorAsync += Error_Handler;

            await _event_client.StartProcessingAsync();

            await Task.Delay(TimeSpan.FromSeconds(100));

            await _event_client.StopProcessingAsync();

            Console.WriteLine("Completed");

        }
        static async Task Process_Message(ProcessEventArgs eventArgs)
        {
            Console.WriteLine("Getting the events");
            Console.WriteLine(Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
            // This will update the checkpoint in the storage account
            // This will ensure this handler will only receive newer events
            await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
        }

        static Task Error_Handler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine("An Error has occurred");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
