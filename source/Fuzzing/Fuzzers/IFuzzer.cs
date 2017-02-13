using System;
using System.Collections.Generic;

namespace Fuzzing.Fuzzers
{
	public interface IFuzzer
	{
		Type FuzzedType { get; }

		object Fuzz(Type type);
		IEnumerable<object> Fuzz(Type type, int count);
	}
}
