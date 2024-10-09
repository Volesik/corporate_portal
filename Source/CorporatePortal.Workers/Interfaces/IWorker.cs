namespace CorporatePortal.Workers.Interfaces;

public interface IWorker
{
    Task ExecuteAsync(int bulkSize);
}