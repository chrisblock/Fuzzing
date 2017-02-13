using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class UnsignedShortIntegerFuzzer : TypeFuzzer<ushort>
	{
		private const int UnsignedShortIntegerSize = sizeof (ushort);

		public override ushort Fuzz()
		{
			var data = new byte[UnsignedShortIntegerSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToUInt16(data, 0);

			return result;
		}
	}
}
