using MassTransit.Definition;
using StackPoint.Domain.Constants;

namespace StackPoint.Service2.MqMassTransit
{
    /// <summary>
    /// Описание потребителя задач на добавление пользователей
    /// </summary>
    public class AddUserConsumerDefinition : ConsumerDefinition<AddUserConsumer>
    {
        public AddUserConsumerDefinition()
        {
            EndpointName = QueueNames.AddUser;
            ConcurrentMessageLimit = 8;
        }
    }
}