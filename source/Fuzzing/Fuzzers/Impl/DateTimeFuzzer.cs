using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class DateTimeFuzzer : TypeFuzzer<DateTime>
	{
		public override DateTime Fuzz()
		{
			var bytes = new byte[8];

			RandomNumberGenerator.GetBytes(bytes);

			var ticks = BitConverter.ToInt64(bytes, 0);

			return new DateTime(ticks);
		}
	}
}
