using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace JeroenPot.WebJob.Twitter
{
    public interface ITwitterRepository
    {
        IList<ITweet> Search(string query, long sinceId);
        void FollowUserIfRequired(ITweet tweet);
        void Retweet(ITweet tweet);
        void Authenticate();
    }
}