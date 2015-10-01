﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using JeroenPot.Common;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Parameters;

namespace JeroenPot.WebJob.Twitter
{
    public class TwitterRepository: ITwitterRepository
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

        public IList<ITweet> Search(string query, long sinceId)
        {
            TweetSearchParameters parameters = new TweetSearchParameters(query);//"(rt AND win)");
            parameters.TweetSearchType = TweetSearchType.OriginalTweetsOnly;
            parameters.SearchType = SearchResultType.Recent;
            parameters.SinceId = sinceId;
            parameters.Lang = Language.Dutch;
            parameters.MaximumNumberOfResults = 100;
            IEnumerable<ITweet> tweets = Tweetinvi.Search.SearchTweets(parameters);

            tweets = tweets.Where(tweet => tweet.Text.StartsWith("RT ", StringComparison.OrdinalIgnoreCase) == false);

            return tweets.Where(tweet => IgnoredUsers.Any(user => tweet.CreatedBy.ScreenName.Equals(user, StringComparison.OrdinalIgnoreCase)) == false).ToList();
        }

        public IList<string> IgnoredUsers
        {
            get
            {
                string users = _configurationRepository.GetAppSetting("IgnoredUsers") ?? string.Empty;
                return users.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
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
}