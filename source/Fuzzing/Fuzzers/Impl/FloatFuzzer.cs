using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class FloatFuzzer : TypeFuzzer<float>
	{
		private const int FloatSize = sizeof (float);

		public override float Fuzz()
		{
			var data = new byte[FloatSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToSingle(data, 0);

			return result;
		}
	}
}
