using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using JeroenPot.Twitter;
using JeroenPot.Website.Models;

namespace JeroenPot.Website.Controllers
{
    public class TwitterController : Controller
    {
        public static DateTime LastChecked;
        public static long LastTweetId;

        private readonly IRetweeter _retweeter;

        public TwitterController(IRetweeter retweeter)
        {
            _retweeter = retweeter;
        }

        public async Task<ActionResult> Retweet()
        {
            var lastTweetId = await _retweeter.RetweetAndWin();
            
            LastChecked = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "W. Europe Standard Time");
            LastTweetId = lastTweetId;

            return RedirectToAction("Index");
        }

        // GET: Twitter
        public ActionResult Index()
        {
            var twitterModel = new TwitterModel();
            twitterModel.LastChecked = LastChecked;
            twitterModel.LastTweetId = LastTweetId;
            return View(twitterModel);
        }
    }
}