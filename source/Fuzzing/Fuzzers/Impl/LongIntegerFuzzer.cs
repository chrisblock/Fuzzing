using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class LongIntegerFuzzer : TypeFuzzer<long>
	{
		private const int LongIntegerSize = sizeof (long);

		public override long Fuzz()
		{
			var data = new byte[LongIntegerSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToInt64(data, 0);

			return result;
		}
	}
}
