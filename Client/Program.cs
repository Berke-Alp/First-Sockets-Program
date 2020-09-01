using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
	class Program
	{
		static int retryCount = 1;

		static void Main(string[] args)
		{
			Console.Title = "Sockets Client";
			Console.Clear();

			// Bir client tanımlıyoruz
			TcpClient tcpc = new TcpClient();

			// Bağlanana kadar 5 kez dene
			while(retryCount != 6)
			{
				try
				{
					// Tanımlanan client ile localhost'un 8085 portuna bağlanıyoruz
					Console.WriteLine("Trying to connect to 127.0.0.1:8085");
					tcpc.Connect("127.0.0.1", 8085);
				}
				catch (SocketException) { }

				if (tcpc.Connected) break;

				if (retryCount == 5) break;
				else if (retryCount == 4) Console.WriteLine($"[{retryCount}] Retrying to connect to localhost one last time...");
				else
				{
					Console.WriteLine($"[{retryCount}] Can't connect to localhost, retrying in 2s...");
					Thread.Sleep(2000);
				}
				retryCount++;
			}

			// Eğer 5 kez yeniden denenmiş ise programı sonlandır
			if(retryCount == 6)
			{
				Console.WriteLine("Couldn't connect to localhost.\nPress any key to exit the program...");
				Console.ReadKey();
				return;
			}

			Console.WriteLine("Connected to localhost...");
			
			try
			{
				Console.Write("Enter your message: ");
				int sentbytes = tcpc.Client.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
				tcpc.Close();
				Console.WriteLine($"-> You sent {sentbytes} bytes to the server. If you want to send another message, press Y.");
				if (Console.ReadKey(false).Key == ConsoleKey.Y) Main(args);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Hata: " + ex.Message);
			}
		}
	}
}
