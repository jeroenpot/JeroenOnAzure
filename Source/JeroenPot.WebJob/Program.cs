using Microsoft.Azure.WebJobs;

namespace JeroenPot.WebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.Call(typeof(Functions).GetMethod("ProcessMethod"));
            host.RunAndBlock();
        }
    }
}
