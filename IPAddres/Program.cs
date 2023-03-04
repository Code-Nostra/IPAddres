using System;
using System.Net;

class Program
{
	static void Main(string[] args)
	{
		Console.Write("Введите колличество IP-адресов: ");
		int count=Convert.ToInt32(Console.ReadLine());

		string[] ipAddreses = new string[count];

		for(int i= 0;i < count; i++)
		{
			Console.Write($"Введите IP-адрес №{i+1}:");
			ipAddreses[i] = Console.ReadLine();
		}



		

		string[] Mask = new string[count];

		for (int i = 0; i < count; i++)
		{
			Console.Write($"Введите маску №{i + 1}:");
			Mask[i] = Console.ReadLine();
		}


		for (int n = 0; n < count; n++)
		{
			Console.WriteLine($"\n{n+1}:\n");

			string[] ipParts = ipAddreses[n].Split('.');
			byte[] ipBytes = new byte[4];

			for (int i = 0; i < 4; i++)
			{
				ipBytes[i] = Convert.ToByte(ipParts[i]);
			}

			//Console.Write("Введите маску подсети в виде количества бит: ");
			int subnetBits = Convert.ToInt32(Mask[n]);

			byte[] subnetMask = new byte[4];
			int fullBytes = subnetBits / 8;

			for (int i = 0; i < fullBytes; i++)
			{
				subnetMask[i] = 0xFF;
			}

			if (subnetBits % 8 != 0)
			{
				subnetMask[fullBytes] = (byte)(0xFF << (8 - subnetBits % 8));
			}

			for (int i = fullBytes + 1; i < 4; i++)
			{
				subnetMask[i] = 0x00;
			}

			byte[] networkAddress = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				networkAddress[i] = (byte)(ipBytes[i] & subnetMask[i]);
			}

			byte[] invertedSubnetMask = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				invertedSubnetMask[i] = (byte)~subnetMask[i];
			}

			byte[] hostAddress = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				hostAddress[i] = (byte)(ipBytes[i] & invertedSubnetMask[i]);
			}

			string binaryIp = string.Join(".", ipParts
				.Select(octet => Convert.ToString(int.Parse(octet), 2).PadLeft(8, '0')));












			Console.WriteLine($"IP-адрес: {string.Join(".", ipBytes)}");
			Console.WriteLine($"IP-адрес в двоичном виде: {binaryIp}");
			Console.WriteLine($"Маска подсети: {string.Join(".", subnetMask)}");
			Console.WriteLine($"Адрес сети: {string.Join(".", networkAddress)}");
			Console.WriteLine($"Адрес узла в сети: {string.Join(".", hostAddress)}");

			// Определение класса сети по первому октету IP-адреса
			string networkClass = "";
			byte firstOctet = ipBytes[0];
			if (firstOctet >= 0 && firstOctet <= 127)
			{
				networkClass = "A";
			}
			else if (firstOctet >= 128 && firstOctet <= 191)
			{
				networkClass = "B";
			}
			else if (firstOctet >= 192 && firstOctet <= 223)
			{
				networkClass = "C";
			}
			else if (firstOctet >= 224 && firstOctet <= 239)
			{
				networkClass = "D";
			}
			else if (firstOctet >= 240 && firstOctet <= 255)
			{
				networkClass = "E";
			}

			Console.WriteLine($"Класс сети: {networkClass}");



			int hostNumber = 0;
			for (int i = 0; i < 4; i++)
			{
				hostNumber += hostAddress[i] << (8 * (3 - i));
			}

			int numberOfHosts = (int)Math.Pow(2, 32 - subnetBits) - 2;
			Console.WriteLine($"Порядковый номер узла в сети: {hostNumber}");


			// Определение широковещательного IP-адреса сети
			byte[] broadcastAddress = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				broadcastAddress[i] = (byte)(networkAddress[i] | invertedSubnetMask[i]);
			}

			Console.WriteLine($"Широковещательный адресв сети: {string.Join(".", broadcastAddress)}");
			
		}
		Console.Read();
	}
}
