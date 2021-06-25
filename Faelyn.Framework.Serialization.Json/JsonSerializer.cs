using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Serialization.Json
{
    public class JsonSerializer : ISerializer
    {
        #region Properties
        public JsonSerializerOptions Options { get; }
        
        #endregion
        
        #region Life cycle

        public JsonSerializer(JsonSerializerOptions options = null)
        {
            Options = options;
        }

        #endregion
        
        #region Methods
        
        public TState Deserialize<TState>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TState>(json, Options);
        }
        
        public TState Deserialize<TState>(byte[] json, Encoding encoding)
        {
            if (encoding != null && !Encoding.UTF8.Equals(encoding))
            {
                throw new ArgumentOutOfRangeException(nameof(encoding));
            }
            return System.Text.Json.JsonSerializer.Deserialize<TState>(json, Options);
        }
        
        public async Task<TState> Deserialize<TState>(Stream json)
        {
            return await System.Text.Json.JsonSerializer.DeserializeAsync<TState>(json, Options);
        }

        public string Serialize<TState>(TState state)
        {
            return System.Text.Json.JsonSerializer.Serialize(state, Options);
        }
        
        public byte[] Serialize<TState>(TState state, Encoding encoding)
        {
            if (encoding != null && !Encoding.UTF8.Equals(encoding))
            {
                throw new ArgumentOutOfRangeException(nameof(encoding));
            }
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(state, Options);
        }
        
        public async Task Serialize<TState>(TState state, Stream json)
        {
            await System.Text.Json.JsonSerializer.SerializeAsync(json, state, Options);
        }
        
        #endregion
    }
}