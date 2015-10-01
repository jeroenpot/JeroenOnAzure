using Microsoft.WindowsAzure.Storage.Table;

namespace JeroenPot.WebJob.Twitter
{
    public class LastTweet : TableEntity
    {
        public LastTweet() { }

        public LastTweet(long lastTweetId)
        {
            PartitionKey = "1";
            RowKey = "1";
            LastTweetId = lastTweetId;
        }

        public long LastTweetId { get; set; }
    }
}