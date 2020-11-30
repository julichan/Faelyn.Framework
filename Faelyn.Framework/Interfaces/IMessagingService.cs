namespace Faelyn.Framework.Interfaces
{
    /// <summary>
    /// This is the interface defining the contract to send and register to message
    /// </summary>
    public interface IMessagingService
    {
        /// <summary>
        /// This method allows a message to be sent to registered classes
        /// </summary>
        void SendMessage<TMessage>(TMessage message);

        /// <summary>
        /// This method allows a listener to listen to a particular type of message.
        /// It is safer to implement registration with weakreferences.
        /// </summary>
        void Register<TMessage>(IMessagingListener<TMessage> listener);

        /// <summary>
        /// This method allows a registered listener to unregister from listening to a particular type of message.
        /// </summary>
        void Unregister<TMessage>(IMessagingListener<TMessage> listener);
    }
}