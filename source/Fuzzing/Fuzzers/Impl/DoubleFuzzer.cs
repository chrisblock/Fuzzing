using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class DoubleFuzzer : TypeFuzzer<double>
	{
		private const int DoubleSize = sizeof(double);

		public override double Fuzz()
		{
			var data = new byte[DoubleSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToDouble(data, 0);

			return result;
		}
	}
}
