namespace Fuzzing.Fuzzers.Impl
{
	public class ByteFuzzer : TypeFuzzer<byte>
	{
		public override byte Fuzz()
		{
			var data = new byte[1];

			RandomNumberGenerator.GetBytes(data);

			return data[0];
		}
	}
}
