using System;
using System.IO;
using System.Text;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Components
{
    public class DataObjectFile<TState> : IDataObjectSource<TState>
    {
        private ISerializer _serializer;
        private Encoding _encoding;
        private string _filepath;

        public DataObjectFile(string filepath, ISerializer serializer, Encoding encoding = null)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _filepath = filepath ?? throw new ArgumentNullException(nameof(filepath));
            _encoding = encoding ?? Encoding.UTF8;
        }

        public TState Load()
        {
            var data = File.ReadAllBytes(_filepath);
            return _serializer.Deserialize<TState>(data, _encoding);
        }

        public void Save(TState state)
        {
            var data = _serializer.Serialize(state, _encoding);
            File.WriteAllBytes(_filepath, data);
        }
    }
}