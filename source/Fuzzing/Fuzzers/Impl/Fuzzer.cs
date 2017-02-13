using System;
using System.Collections.Generic;
using System.Linq;

namespace Fuzzing.Fuzzers.Impl
{
	public class Fuzzer : IFuzzer
	{
		private readonly IFuzzer _defaultFuzzer;
		private readonly IFuzzer _enumFuzzer;
		private readonly IDictionary<Type, IFuzzer> _fuzzers;

		public Fuzzer()
		{
			_defaultFuzzer = new ReflectionFuzzer(this);
			_enumFuzzer = new EnumFuzzer();

			var typesAssignableFromIFuzzer = AppDomain.CurrentDomain
				.GetAssemblies()
				.Where(x => x.GlobalAssemblyCache == false)
				.Where(x => x.IsDynamic == false)
				.SelectMany(x => x.GetExportedTypes())
				.Where(x => (x.IsInterface == false) && (x.IsAbstract == false))
				.Where(x => typeof (IFuzzer).IsAssignableFrom(x))
				.Where(x => typeof (Fuzzer) != x)
				.Where(x => typeof (ReflectionFuzzer) != x)
				.Where(x => typeof (EnumFuzzer) != x);

			_fuzzers = typesAssignableFromIFuzzer
				.Select(x => (IFuzzer) Activator.CreateInstance(x))
				.Where(x => x.FuzzedType != null)
				.ToDictionary(x => x.FuzzedType, x => x);
		}

		public Type FuzzedType
		{
			get { throw new NotImplementedException(); }
		}

		public object Fuzz(Type type)
		{
			IFuzzer fuzzer;

			if (_fuzzers.TryGetValue(type, out fuzzer) == false)
			{
				if (type.IsPrimitive)
				{
					throw new ArgumentException(String.Format("Cannot fuzz primitive type '{0}'. It is unrecognized.", type.Name));
				}

				if (type.IsPointer)
				{
					throw new NotSupportedException(String.Format("Cannot fuzz pointer type '{0}'. The fuzzing of pointers is unsupported.", type.Name));
				}

				fuzzer = type.IsEnum
					? _enumFuzzer
					: _defaultFuzzer;
			}

			var result = fuzzer.Fuzz(type);

			return result;
		}

		public IEnumerable<object> Fuzz(Type type, int count)
		{
			return Enumerable.Range(0, count)
				.Select(x => Fuzz(type));
		}
	}
}
