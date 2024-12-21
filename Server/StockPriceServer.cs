using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marketContext;

namespace StockPriceServer
{
    public class TcpServer
    {
        private const int Port = 12345;
        private readonly MarketContext _dbContext;

        public TcpServer(MarketContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task StartAsync()
        {
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("Сервер запущен...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                var buffer = new byte[1024];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                var request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Получен запрос на тикер: {request}");

                // Найти последнюю цену акции
                var latestPrice = await _dbContext.Prices
                    .Where(p => p.tickerSymPrices.tickerSym == request)
                    .OrderByDescending(p => p.date)
                    .FirstOrDefaultAsync();

                if (latestPrice != null)
                {
                    var response = latestPrice.price.ToString("F2"); // Отправить последнюю цену с точностью до двух знаков после запятой
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine($"Отправлена цена акции {request}: {response}");
                }
                else
                {
                    var responseBytes = Encoding.UTF8.GetBytes("Нет данных");
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine($"Нет данных для тикера {request}");
                }
            }
        }
    }

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using (var dbContext = new MarketContext())
            {
                var server = new TcpServer(dbContext);
                await server.StartAsync();
            }
        }
    }
}
