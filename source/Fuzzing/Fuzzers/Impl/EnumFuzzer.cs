using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuzzing.Fuzzers.Impl
{
	public class EnumFuzzer : IFuzzer
	{
		private readonly UnsignedIntegerFuzzer _intFuzzer = new UnsignedIntegerFuzzer();

		public Type FuzzedType { get { return null; } }

		public object Fuzz(Type type)
		{
			var values = Enum.GetValues(type);

			var count = values.Length;

			var i = _intFuzzer.Fuzz() % count;

			return values.GetValue(i);
		}

		public IEnumerable<object> Fuzz(Type type, int count)
		{
			return Enumerable.Range(0, count)
				.Select(x => Fuzz(type));
		}
	}
}
