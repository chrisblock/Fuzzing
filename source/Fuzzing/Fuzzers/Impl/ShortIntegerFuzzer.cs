using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class ShortIntegerFuzzer : TypeFuzzer<short>
	{
		private const int ShortIntegerSize = sizeof (short);

		public override short Fuzz()
		{
			var data = new byte[ShortIntegerSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToInt16(data, 0);

			return result;
		}
	}
}
