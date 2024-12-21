using System;
using System.Net.Sockets;
using System.Text;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                // Ввод тикера
                Console.Write("Введите тикер: ");
                string ticker = Console.ReadLine();

                try
                {
                    using TcpClient client = new TcpClient("192.168.56.1", 12345);
                    using NetworkStream stream = client.GetStream();

                    // Отправляем тикер серверу
                    byte[] data = Encoding.UTF8.GetBytes(ticker);
                    stream.Write(data, 0, data.Length);

                    // Получаем ответ от сервера
                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Console.WriteLine($"Ответ сервера: {response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                // Запрос продолжения
                Console.Write("Хотите продолжить? (Y/N): ");
                string continueResponse = Console.ReadLine();
                if (continueResponse.ToUpper() != "Y")
                {
                    break;
                }

            } while (true);

            // Ожидание нажатия клавиши перед выходом
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
