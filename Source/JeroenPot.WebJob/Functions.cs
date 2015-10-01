using System;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.Azure.WebJobs;

namespace JeroenPot.WebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static void ProcessMethod(TextWriter log)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("hello!");
                }
                catch (Exception ex)
                {
                    log.WriteLine("Error occurred {0}", ex);
                }


                try
                {
                    WebRequest.Create("http://http://dev-jeroen.azurewebsites.net/");
                }
                catch (Exception)
                {
                    
                    throw;
                }
                Thread.Sleep(TimeSpan.FromMinutes(10));
            }
        }
    }
}
