using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Yu.Tookit.SocketsManager
{
    public class ConnectionManager
    {
        readonly ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value;
        }
        public ConcurrentDictionary<string, WebSocket> GetAllConnections()
        {
            return _connections;
        }
        public string GetId(WebSocket socket)
        {
            return _connections.FirstOrDefault(x => x.Value == socket).Key;
        }
        public async Task RemoveSocketAsync(string id)
        {
            _connections.TryRemove(id, out WebSocket socket);
            if (socket.State != WebSocketState.Aborted)
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed", CancellationToken.None);
        }
        public void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionId(), socket);
        }
        string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
