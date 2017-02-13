using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class IntegerFuzzer : TypeFuzzer<int>
	{
		private const int IntegerSize = sizeof (int);

		public override int Fuzz()
		{
			var data = new byte[IntegerSize];
			RandomNumberGenerator.GetBytes(data);

			var result = BitConverter.ToInt32(data, 0);

			return result;
		}
	}
}
