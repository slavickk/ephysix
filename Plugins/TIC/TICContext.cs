using System.Net.Sockets;

namespace ParserLibrary;

public class TICContext
{
    public TcpClient Client { get; set; }
    public Guid MessageID { get; set; } = Guid.NewGuid();
    public CancellationToken CancellationToken { get; set; } = default;
}