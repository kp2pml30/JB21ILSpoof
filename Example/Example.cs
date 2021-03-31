using System;
using System.Collections.Generic;

namespace Example
{
	public class Program
	{
		private static int x = 1;
		int y = 30;

		IDictionary<string, string> result = new SortedDictionary<string, string>();

		public int Prop
		{
			get => y + 1;
			set => y = y + value;
		}

		private void Add(string a, object b)
		{
			result[a] = b.ToString();
		}

		private IDictionary<string, string> TestImpl()
		{
			TestInt();
			TestDouble();
			TestDecimal();
			TestProperty();
			return result;
		}

		public static IDictionary<string, string> Test()
		{
			var self = new Program();
			return self.TestImpl();
		}

		public static void Main(string[] args)
		{
			foreach (var i in Test())
			{
				Console.WriteLine(i.Key);
				Console.WriteLine(i.Value);
			}
		}

		private void TestInt()
		{
			var a = 2;
			var b = x * 2;
			Add("Int", a + b);
		}

		private void TestDouble()
		{
			var a = 5.0;
			double b = x * 3;

			Add("Double", a + b);
		}

		private void TestDecimal()
		{
			var a = 30m;
			decimal b = x * 4;

			Add("Decimal", a + b);
		}

		private void TestProperty()
		{
			Add("GetProperty", Prop);

			Prop = 3;
			Add("SetProperty", Prop);
		}
	}
}
