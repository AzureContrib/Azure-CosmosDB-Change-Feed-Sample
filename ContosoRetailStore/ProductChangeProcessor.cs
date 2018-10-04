using Microsoft.Azure.Documents.ChangeFeedProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contoso.ProcessorConsole
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductChangeProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        public static void RunProcessing()
        {
            RunProcessingAsync().Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static async Task RunProcessingAsync()
        {
            DocumentCollectionInfo feedCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "DatabaseName",
                CollectionName = "MonitoredCollectionName",
                Uri = new Uri("https://sampleservice.documents.azure.com:443/"),
                MasterKey = "<authKey>"
            };

            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "DatabaseName",
                CollectionName = "leases",
                Uri = new Uri("https://sampleservice.documents.azure.com:443/"),
                MasterKey = "-- the auth key"
            };

            var builder = new ChangeFeedProcessorBuilder();
            var processor = await builder
                .WithHostName("ProductChangeObserverHost")
                .WithFeedCollection(feedCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithObserver<ProductChangeObserver>()
                .BuildAsync();

            await processor.StartAsync();

            Console.WriteLine("Change Feed Processor started. Press <Enter> key to stop...");
            Console.ReadLine();

            await processor.StopAsync();
        }
    }
}
