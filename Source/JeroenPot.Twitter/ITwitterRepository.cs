using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;

namespace JeroenPot.Twitter
{
    public interface ITwitterRepository
    {
        TweetContainer Search(string query, long sinceId, int maximumNumberOfResults);
        void FollowUserIfRequired(ITweet tweet);
        void Retweet(ITweet tweet);
        void Authenticate();
    }
}