using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class DecimalFuzzer : TypeFuzzer<decimal>
	{
		public override decimal Fuzz()
		{
			var ints = new int[3];
			var bytes = new byte[14];
			RandomNumberGenerator.GetBytes(bytes);

			for (var i = 0; i < ints.Length; i++)
			{
				ints[i] = BitConverter.ToInt32(bytes, sizeof (int) * i);
			}

			return new Decimal(ints[0], ints[1], ints[2], BitConverter.ToBoolean(bytes, 12), (byte) (bytes[13] % 29));
		}
	}
}
