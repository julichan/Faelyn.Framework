namespace Faelyn.Framework.Interfaces
{
    /// <typeparam name="TState"></typeparam>
    public interface IDataObjectRef<out TState> : INotifyStateChanges, IReceiveStateChanges
    {
        #region Properties
        
        /// <summary>
        /// This property contains the actual data.
        /// </summary>
        TState CurrentData { get; }

        #endregion Properties
    }
}