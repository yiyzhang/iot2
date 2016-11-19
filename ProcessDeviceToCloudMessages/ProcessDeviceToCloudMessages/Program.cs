using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ProcessDeviceToCloudMessages
{
    class Program
    {
        static void Main(string[] args)
        {
            string iotHubConnectionString = "HostName=JZ-HUB1.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=c5z3GWBRpQekSqu6s1rQMX46QtoTNJzneJZKzmwYpmc=";
            string iotHubD2cEndpoint = "messages/events";
            StoreEventProcessor.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=jziotstorage1;AccountKey=/Oo9KCoyXNZDBe5WB3Rn4fXA0IeWXUMFk7+7j2Io33OIVMBT6F+XeWVEhXHZFFY3OJMP+fJL8RzMD2ucZ1fnRQ==;";
            StoreEventProcessor.ServiceBusConnectionString = "Endpoint=sb://jziotservicebus1.servicebus.windows.net/;SharedAccessKeyName=send;SharedAccessKey=45qtrkYcwn3OSbTy30nlMzfGsAbLNxuAOcGUYOtDMys=;EntityPath=queue1";
                                                                
            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, iotHubD2cEndpoint, EventHubConsumerGroup.DefaultGroupName, iotHubConnectionString, StoreEventProcessor.StorageConnectionString, "messages-events");
            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<StoreEventProcessor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
