using Microsoft.Azure.Documents.ChangeFeedProcessor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contoso.ProcessorConsole.NETFx
{
    /// <summary>
    ///  This class works as the processor of all change feeds
    /// </summary>
    public class ProductChangeProcessor
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ChangeFeedProcessorBuilder builder = new ChangeFeedProcessorBuilder();
        /// <summary>
        ///  Start Processing the Changes
        /// </summary>
        public static void DoProcessing()
        {
            DoProcessingAsync().Wait();
        }

        static async Task DoProcessingAsync()
        {
            string hostName = Guid.NewGuid().ToString();
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
                CollectionName = "Clothing",
                Uri = new Uri(Constants.CosmosDb_Uri),
                MasterKey = Constants.CosmosDb_Key
               
            };

            var builder = new ChangeFeedProcessorBuilder();

        
            ChangeFeedProcessorOptions feedProcessorOptions = new ChangeFeedProcessorOptions();
            feedProcessorOptions.LeaseRenewInterval = TimeSpan.FromSeconds(15);
            feedProcessorOptions.StartFromBeginning = true;

            //ChangeFeedEventHost host = new ChangeFeedEventHost("ProductChangeObserverHost", feedCollectionInfo, leaseCollectionInfo, feedOptions, feedHostOptions);

            var processor = await builder
                .WithHostName("ProductChangeObserverHost-" + hostName)
                .WithFeedCollection(feedCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                //.WithObserver<ProductChangeObserver>()    
                .WithProcessorOptions(feedProcessorOptions)
                .WithObserverFactory(new ProductFeedObserverFactory()) 
                .BuildAsync();


            await processor.StartAsync();

            Console.WriteLine("Change Feed Processor started. Press <Enter> key to stop...");
            Console.ReadLine();

            await processor.StopAsync();
        }
    }
}
