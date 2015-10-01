using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JeroenPot.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace JeroenPot.WebJob.Twitter
{
    public class TableStorageRepository : ITableStorageRepository
    {
        private readonly IConfigurationRepository _configurationRepository;

        public TableStorageRepository(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        private CloudTable GetTable()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(_configurationRepository.GetAzureWebJobsStorageConnection());

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("twitter");
            table.CreateIfNotExists();
            return table;
        }

        public async Task Save(LastTweet lastTweet)
        {
            CloudTable table = GetTable();

            // Create the InsertOrReplace TableOperation.
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(lastTweet);

            // Execute the operation.
            await table.ExecuteAsync(insertOrReplaceOperation);
        }

        public LastTweet GetLastTweet()
        {
            var table = GetTable();

            TableQuery<LastTweet> query =
                new TableQuery<LastTweet>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                    QueryComparisons.Equal, "1"));

            LastTweet lastTweet = table.ExecuteQuery(query).FirstOrDefault();

            if (lastTweet == null)
            {
                lastTweet = new LastTweet(649474600812834816);
            }

            return lastTweet;
        }
    }
}


