using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Fuzzing.Fuzzers
{
	public abstract class TypeFuzzer<T> : IFuzzer
	{
		protected RandomNumberGenerator RandomNumberGenerator { get; private set; }

		protected TypeFuzzer()
		{
			RandomNumberGenerator = RandomNumberGenerator.Create();
		}

		public bool CanFuzzType(Type type)
		{
			var result = (typeof (T) == type);

			return result;
		}

		public Type FuzzedType { get { return typeof (T); } }

		public object Fuzz(Type type)
		{
			return Fuzz();
		}

		public IEnumerable<object> Fuzz(Type type, int count)
		{
			return Fuzz(count)
				.Cast<object>();
		}

		public abstract T Fuzz();

		public IEnumerable<T> Fuzz(int count)
		{
			return Enumerable.Range(0, count)
				.Select(x => Fuzz());
		}
	}
}
