using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Faelyn.Framework.Interfaces
{
    public interface ISerializer
    {
        TState Deserialize<TState>(string json);

        TState Deserialize<TState>(byte[] json, Encoding encoding);

        Task<TState> Deserialize<TState>(Stream json);

        string Serialize<TState>(TState state);

        byte[] Serialize<TState>(TState state, Encoding encoding);

        Task Serialize<TState>(TState state, Stream json);
    }
}