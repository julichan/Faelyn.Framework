namespace Faelyn.Framework.Interfaces
{
    public interface IDataObjectSource<TState>
    {
        TState Load();

        void Save(TState dataObject);
    }
} 