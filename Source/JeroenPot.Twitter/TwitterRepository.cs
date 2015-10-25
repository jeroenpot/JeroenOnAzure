using System;
using System.Collections.Generic;
using System.Linq;
using JeroenPot.Common;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Parameters;

namespace JeroenPot.Twitter
{
    public class TwitterRepository : ITwitterRepository
    {
        private readonly IConfigurationRepository _configurationRepository;

        public TwitterRepository(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public void Authenticate()
        {
            string consumerKey = _configurationRepository.GetAppSetting("TwitterConsumerKey");
            string consumerSecret = _configurationRepository.GetAppSetting("TwitterConsumerSecret");
            string accessToken = _configurationRepository.GetAppSetting("TwitterAccessToken");
            string accessTokenSecret = _configurationRepository.GetAppSetting("TwitterAccessTokenSecret");

            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
            Auth.InitializeApplicationOnlyCredentials();
        }

        public TweetContainer Search(string query, long sinceId, int maximumNumberOfResults)
        {
            TweetContainer container = new TweetContainer();
            TweetSearchParameters parameters = new TweetSearchParameters(query);
            parameters.TweetSearchType = TweetSearchType.OriginalTweetsOnly;
            parameters.SearchType = SearchResultType.Mixed;
            parameters.SinceId = sinceId;
            parameters.MaximumNumberOfResults = maximumNumberOfResults;
            container.Tweets = Tweetinvi.Search.SearchTweets(parameters);
            container.HasNextBatch = maximumNumberOfResults == container.Tweets.Count();

            container.Tweets = container.Tweets.Where(tweet => tweet.Text.IndexOf(": RT", StringComparison.OrdinalIgnoreCase) > -1 == false);
            container.Tweets = container.Tweets.Where(tweet => tweet.Text.StartsWith("RT", StringComparison.OrdinalIgnoreCase) == false);
            container.Tweets = container.Tweets.Where(tweet => IgnoredUsers.Any(user => !tweet.CreatedBy.ScreenName.Equals(user, StringComparison.OrdinalIgnoreCase)));
            container.Tweets = container.Tweets.Where(tweet => IgnoredUsers.Any(user => !tweet.Text.Contains(user)));
            container.Tweets = container.Tweets.Where(tweet => tweet.Text.Replace(".", "").Replace(" ", "").IndexOf("lastrtwin", StringComparison.OrdinalIgnoreCase) == -1);

            return container;
        }

        public IList<string> IgnoredUsers
        {
            get
            {
                string users = _configurationRepository.GetAppSetting("IgnoredUsers") ?? string.Empty;
                return users.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            }
        }

        public void FollowUserIfRequired(ITweet tweet)
        {
            if (FollowWords.Any(word => tweet.Text.IndexOf(word, StringComparison.OrdinalIgnoreCase) > -1))
            {
                try
                {
                    User.GetLoggedUser().FollowUser(tweet.CreatedBy.Id);
                }
                catch (Exception)
                {
                    // Ignore exceptions.
                }
            }

        }

        private static List<string> FollowWords
        {
            get { return new List<string>() { "VOLG", "FOLLOW", "FLW" }; }
        }

        public void Retweet(ITweet tweet)
        {
            tweet.PublishRetweet();
        }
    }

    public class TweetContainer
    {
        public bool HasNextBatch { get; set; }
        public IEnumerable<ITweet> Tweets { get; set; }
    }
}
