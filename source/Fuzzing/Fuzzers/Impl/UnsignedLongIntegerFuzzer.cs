using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class UnsignedLongIntegerFuzzer : TypeFuzzer<ulong>
	{
		private const int UnsignedLongIntegerSize = sizeof (ulong);

		public override ulong Fuzz()
		{
			var data = new byte[UnsignedLongIntegerSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToUInt64(data, 0);

			return result;
		}
	}
}
