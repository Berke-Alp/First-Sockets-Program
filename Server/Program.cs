using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "Sockets";
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("Listening on port 8085...");

			TcpListener listener = new TcpListener(new IPAddress(0), 8085);

			while (true)
			{
				try
				{
					// Dinleyici dinlemeye başlar
					listener.Start();
					// Gelen veriyi göndereni client olarak tanımlıyoruz
					TcpClient client = listener.AcceptTcpClient();

					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("Incoming connection: ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(client.Client.RemoteEndPoint.ToString());

					// Gelen veri NetworkStream olduğu için bu Stream'i okuyoruz
					StreamReader reader = new StreamReader(client.GetStream());
					// Gelen veriyi tümüyle ekrana basıyoruz
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine(reader.ReadToEnd());
					Console.WriteLine();

					// Okuyucuyu durduruyoruz
					reader.Close();
					// Dinlemeyi bırakıyoruz
					listener.Stop();
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Hata oluştu: " + ex.Message);
					break;
				} 
			}

			Console.ReadKey();
		}
	}
}
