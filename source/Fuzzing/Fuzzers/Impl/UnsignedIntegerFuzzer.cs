using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class UnsignedIntegerFuzzer : TypeFuzzer<uint>
	{
		private const int UnsignedIntegerSize = sizeof(uint);

		public override uint Fuzz()
		{
			var data = new byte[UnsignedIntegerSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToUInt32(data, 0);

			return result;
		}
	}
}
