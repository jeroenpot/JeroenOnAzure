using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JeroenPot.Common;
using JeroenPot.WebJob.Twitter;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;

namespace JeroenPot.WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static async Task ProcessMethod(TextWriter log)
        {
            while (true)
            {
                try
                {
                    IRetweeter retweeter = new Retweeter(new TableStorageRepository(new ConfigurationRepository()), new TwitterRepository(new ConfigurationRepository()));
                    retweeter.RetweetAndWin();
                    await new WebsiteRepository().MakeRequest(new Uri("http://jeroenonazure.azurewebsites.net/"));
                }
                catch (Exception ex)
                {
                    log.WriteLine("Error occurred {0}", ex);
                }

                await Task.Delay(TimeSpan.FromMinutes(10));
            }
        }
    }
}
