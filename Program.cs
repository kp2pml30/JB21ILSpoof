using System;
using System.IO;

namespace CSharpPlusSpoof
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length != 2)
			{
				Console.Error.WriteLine("Usage: <input> <output>");
				return;
			}
			string inPath = args[0];
			string outPath = args[1];

			Spoofer spoof;
			try
			{
				using (Stream istrm = File.Open(inPath, FileMode.Open))
				{
					spoof = new Spoofer(istrm);

					spoof.Process();

					using (Stream ostrm = File.Create(outPath))
						spoof.Write(ostrm);
				}
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine($"File {inPath} not found\n\n{e}");
				return;
			}
			catch (IOException e)
			{
				Console.WriteLine($"IO exception during writing {outPath}\n\n{e}");
				return;
			}
		}
	}
}
