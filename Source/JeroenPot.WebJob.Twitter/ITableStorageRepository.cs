using System.Threading.Tasks;

namespace JeroenPot.WebJob.Twitter
{
    public interface ITableStorageRepository
    {
        Task Save(LastTweet lastTweet);
        LastTweet GetLastTweet();
    }
}