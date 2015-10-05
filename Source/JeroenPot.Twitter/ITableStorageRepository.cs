using System.Threading.Tasks;

namespace JeroenPot.Twitter
{
    public interface ITableStorageRepository
    {
        Task Save(LastTweet lastTweet);
        LastTweet GetLastTweet();
    }
}