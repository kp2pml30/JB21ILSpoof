using System;

namespace Example
{
	class Program
	{
		private static int x = 1;
		int y = 30;

		public int Prop
		{
			get => y + 1;
			set => y = y + value;
		}

		static void Main(string[] args)
		{
			TestInt();
			TestDouble();
			TestDecimal();
			var self = new Program();
			self.TestMethod();
			self.TestProperty();
		}

		private static void TestInt()
		{
			var a = 2;
			var b = x * 2;
			Console.WriteLine("Int");
			Console.WriteLine(a + b);
		}

		private static void TestDouble()
		{
			var a = 5.0;
			double b = x * 3;

			Console.WriteLine("Double");
			Console.WriteLine(a + b);
		}

		private static void TestDecimal()
		{
			var a = 30m;
			decimal b = x * 4;

			Console.WriteLine("Decimal");
			Console.WriteLine(a + b);
		}

		private void TestMethod(int a = -5)
		{
			Console.WriteLine("Method");
			Console.WriteLine(a + x + 40);
		}
		private void TestProperty()
		{
			Console.WriteLine("GetProperty");
			Console.WriteLine(Prop);

			Prop = 3;
			Console.WriteLine("SetProperty");
			Console.WriteLine(Prop);
		}
	}
}
