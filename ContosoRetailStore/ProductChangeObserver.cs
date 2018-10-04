using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;

namespace Contoso.ProcessorConsole
{
    public class ProductChangeObserver : IChangeFeedObserver
    {
        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            return Task.CompletedTask;  
        }

        public Task OpenAsync(IChangeFeedObserverContext context)
        {
            return Task.CompletedTask;
        }

        public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> docs, CancellationToken cancellationToken)
        {
            Console.WriteLine("ProcessChangesAsync: partition {0}, {1} docs", context.PartitionKeyRangeId, docs.Count);
            return Task.CompletedTask;
        }
    }
}
