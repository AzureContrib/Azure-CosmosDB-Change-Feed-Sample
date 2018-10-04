using Microsoft.Azure.Documents.ChangeFeedProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contoso.ProcessorConsole.NETFx
{
    /// <summary>
    ///  This class works as the processor of all change feeds
    /// </summary>
    public class ProductChangeProcessor
    {
        /// <summary>
        ///  Start Processing the Changes
        /// </summary>
        public static void DoProcessing()
        {
            DoProcessingAsync().Wait();
        }

        static async Task DoProcessingAsync()
        {
            DocumentCollectionInfo feedCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = Constants.CosmosDb_DatabaseName,
                CollectionName = Constants.CosmosDb_CollectionName,
                Uri = new Uri(Constants.CosmosDb_Uri),
                MasterKey = Constants.CosmosDb_Key
            };

            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = Constants.CosmosDb_DatabaseName,
                CollectionName = "leases",
                Uri = new Uri(Constants.CosmosDb_Uri),
                MasterKey = Constants.CosmosDb_Key
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
