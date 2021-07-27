namespace StackPoint.Domain.Services
{
    public interface IMqSender
    {
        bool Send<T>(string queueName, T entity) where T : class;
    }
}