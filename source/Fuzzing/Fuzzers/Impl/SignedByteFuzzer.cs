namespace Fuzzing.Fuzzers.Impl
{
	public class SignedByteFuzzer : TypeFuzzer<sbyte>
	{
		private const int SignedByteSize = sizeof(sbyte);

		public override sbyte Fuzz()
		{
			var data = new byte[SignedByteSize];
			RandomNumberGenerator.GetBytes(data);

			return (sbyte) data[0];
		}
	}
}
