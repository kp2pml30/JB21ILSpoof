using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using ILSpoof;

#nullable enable

namespace Test
{
	class Program
	{
		static readonly IDictionary<string, string> expectedDict = new Dictionary<string, string>
		{
			{"Int", "0"},
			{"Double", "2"},
			{"Decimal", "26"},
			{"GetProperty", "29"},
			{"SetProperty", "26"},
		};

	private static string Dir()
		{
			var path = Assembly.GetExecutingAssembly().Location;
			return System.IO.Path.GetDirectoryName(path);
		}
		private static IDictionary<string, string> GetExample(string dllname)
		{
			var dll = System.Reflection.Assembly.LoadFile(Dir() + Path.DirectorySeparatorChar + dllname);
			var program = dll.GetType("Example.Program");
			if (program == null)
				throw new Exception("no Program in dll");
			var method = program.GetMethod("Test", new Type[] {});
			if (!typeof(IDictionary<string, string>).IsAssignableFrom(method.ReturnType))
				throw new Exception($"bad return in {method}");
			var result = method.Invoke(null, new object[] { }) as IDictionary<string, string>;
			if (result == null)
				throw new NullReferenceException("bad example return");
			return result;
		}
		static void Print(IDictionary<string, string> dict)
		{
			foreach (var i in dict)
			{
				Console.WriteLine(i.Key);
				Console.WriteLine(i.Value);
			}
		}

		static void PrintDiff(IDictionary<string, string> expected, IDictionary<string, string> got)
		{
			SetForeground(ConsoleColor.Red, () =>
				{
					Console.ForegroundColor = ConsoleColor.Red;
					foreach (var i in expected)
					{
						string? j;
						if (!got.TryGetValue(i.Key, out j))
							Console.WriteLine($"{i.Key} is absent\n\t exp: {i.Value}");
						else if (i.Value != j)
							Console.WriteLine($"{i.Key} differs\n\t exp: {i.Value}\n\t got: {j}");
					}
				});
			SetForeground(ConsoleColor.Yellow, () =>
				{
					foreach (var i in got)
						if (!expected.ContainsKey(i.Key))
							Console.WriteLine($"unknown key {i.Key}");
				});
		}

		static void SetForeground(ConsoleColor color, Action func)
		{
			var oldForeground = Console.ForegroundColor;
			try
			{
				Console.ForegroundColor = color;
				func();
			}
			finally
			{
				Console.ForegroundColor = oldForeground;
			}
		}

		static void Main(string[] args)
		{
			Directory.CreateDirectory("spoofed");
			const string  inp = "Example.dll";
			const string outp = "spoofed/Result.dll";

			using (Stream istrm = File.Open(inp, FileMode.Open), ostrm = File.Create(outp))
			{
				var spoof = new Spoofer(istrm);
				spoof.Process();
				spoof.Write(ostrm);
			}
			SetForeground(ConsoleColor.Green, () => Console.WriteLine("before"));
			Print(GetExample(inp));
			SetForeground(ConsoleColor.Green, () => Console.WriteLine("after patch"));
			Console.WriteLine("after patch");
			var result = GetExample(outp);
			Print(GetExample(outp));
			SetForeground(ConsoleColor.Green, () => Console.WriteLine("difference"));
			PrintDiff(expectedDict, result);
		}
	}
}
