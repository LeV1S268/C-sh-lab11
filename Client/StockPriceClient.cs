using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockPriceClient
{
    internal class Program
    {
        private const string ServerIp = "192.168.56.1"; // Адрес сервера
        private const int ServerPort = 12345; // Порт, на котором работает сервер

        private static async Task Main(string[] args)
        {
            Console.Write("Введите тикер: ");
            var ticker = Console.ReadLine();

            using (var client = new TcpClient(ServerIp, ServerPort))
            {
                var stream = client.GetStream();
                var requestBytes = Encoding.UTF8.GetBytes(ticker);
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                var buffer = new byte[1024];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Последняя цена акции {ticker}: {response}");
            }
        }
    }
}
