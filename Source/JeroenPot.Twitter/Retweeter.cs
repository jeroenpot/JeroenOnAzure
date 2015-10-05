using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Interfaces;

namespace JeroenPot.Twitter
{
    public interface IRetweeter
    {
        Task<long> RetweetAndWin();
    }

    public class Retweeter : IRetweeter
    {
        private readonly ITableStorageRepository _tableStorageRepository;
        private readonly ITwitterRepository _twitterRepository;

        public Retweeter(ITableStorageRepository tableStorageRepository, ITwitterRepository twitterRepository)
        {
            _tableStorageRepository = tableStorageRepository;
            _twitterRepository = twitterRepository;
        }

        public async Task<long> RetweetAndWin()
        {
            int batchsize = 10;

            LastTweet lastRetweet;
            while (true)
            {
                lastRetweet = _tableStorageRepository.GetLastTweet();
                _twitterRepository.Authenticate();
                IList<ITweet> tweets = _twitterRepository.Search("rt win", lastRetweet.LastTweetId, batchsize);

                Console.WriteLine("Found {0} tweets", tweets.Count());

                if (tweets.Any())
                {
                    foreach (var tweet in tweets)
                    {
                        _twitterRepository.FollowUserIfRequired(tweet);
                        _twitterRepository.Retweet(tweet);

                        if (tweet.Id > lastRetweet.LastTweetId)
                        {
                            lastRetweet.LastTweetId = tweet.Id;
                        }
                    }

                    await _tableStorageRepository.Save(lastRetweet);
                }

                if (tweets.Count() < batchsize)
                {
                    break;
                }
            }

            return lastRetweet.LastTweetId;
        }
    }
}
