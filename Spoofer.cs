using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CSharpPlusSpoof
{
	class Spoofer
	{
		private AssemblyDefinition assembly;

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

		public void Process()
		{
			var instructions =
				(from module in assembly.Modules
				 from type in module.Types
				 from method in type.Methods
				 where method.HasBody
				 from instuction in method.Body.Instructions
				 where instuction.OpCode == OpCodes.Add
				 select instuction);
			
			foreach (var i in instructions)
				i.OpCode = OpCodes.Sub;
		}
	}
}
