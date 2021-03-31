using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

#nullable enable

namespace ILSpoof
{
	public class Spoofer
	{
		/// Cecil assembly for corresponding input
		private AssemblyDefinition assembly;

		/// internal field for lazy getting of reference to decimal addition
		private static MethodReference? _decimalSub = null;

		/// property
		private MethodReference DecimalSub
		{
			get
			{
				_decimalSub ??= ImportDecimalSub();
				return _decimalSub;
			}
		}

		/// helper function to simplify lazy allocation
		private MethodReference ImportDecimalSub()
		{
			var dec = typeof(Decimal);
			var info = dec.GetMethod("op_Subtraction", new Type[] { dec, dec });
			return assembly.MainModule.ImportReference(info);
		}

		/// Loads module to "modify"
		///
		/// <param name="input">module input stream. Do not close before <see cref="Write(System.IO.Stream)"/></param>
		public Spoofer(System.IO.Stream input) => assembly = AssemblyDefinition.ReadAssembly(input);

		/// Stores modified module
		///
		/// <param name="output">module output stream</param>
		public void Write(System.IO.Stream output)
		{
			assembly.Write(output);
		}

		/// Actually spoofs opcodes
		public void Process()
		{
			var instructions =
				(from module in assembly.Modules
				 from type in module.Types
				 from method in type.Methods
				 where method.HasBody
				 from instruction in method.Body.Instructions
				 where instruction.OpCode == OpCodes.Add || instruction.OpCode == OpCodes.Call
				 select instruction);
			
			foreach (var i in instructions)
			{
				if (i.OpCode == OpCodes.Add)
					i.OpCode = OpCodes.Sub;
				else if (i.OpCode == OpCodes.Call)
				{
					var obj = i.Operand;
					// I am not sure if it is a good approach
					// `is not null` not available
					if (obj != null && obj.ToString() == "System.Decimal System.Decimal::op_Addition(System.Decimal,System.Decimal)")
						i.Operand = DecimalSub;
				}
			}
		}
	}
}
