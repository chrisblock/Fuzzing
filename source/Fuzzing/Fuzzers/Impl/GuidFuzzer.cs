using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class GuidFuzzer : TypeFuzzer<Guid>
	{
		public override Guid Fuzz()
		{
			return Guid.NewGuid();
		}
	}
}
