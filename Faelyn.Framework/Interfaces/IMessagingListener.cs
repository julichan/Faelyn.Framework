namespace Faelyn.Framework.Interfaces
{
    /// <summary>
    /// This is the interface to implement to listen for a class to listen to the messaging system
    /// </summary>
    /// <typeparam name="TMessage">The type of messge to listen to</typeparam>
    public interface IMessagingListener<TMessage>
    {
        void ReceiveMessage(TMessage message);
    }
}