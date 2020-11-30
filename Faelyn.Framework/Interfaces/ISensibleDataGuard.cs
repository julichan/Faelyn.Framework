using System;

namespace Faelyn.Framework.Interfaces
{

    public interface ISensibleDataGuard
    {
        event EventHandler OnSensibleDataChanged;
        
        void ProtectRawAction(Action<byte[]> action);
        
        void ProtectStringAction(Action<string> action);
        
        TReturn ProtectRawFunction<TReturn>(Func<byte[], TReturn> action);
        
        TReturn ProtectStringFunction<TReturn>(Func<string, TReturn> action);

        void SetRawData(Func<byte[]> input);
        
        void SetStringData(Func<string> input);

        void ClearData();
    }
    
    public interface ISensibleDataGuard<TEncryptedData> : ISensibleDataGuard
    {
        TEncryptedData EncryptedData { get; }
    }
}