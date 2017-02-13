using System;
using System.Collections.Generic;
using System.Linq;

using Fuzzing.Fuzzers;
using Fuzzing.Fuzzers.Impl;

namespace Fuzzing
{
	public static class ObjectFuzzer
	{
		private static readonly IFuzzer Fuzzer = new Fuzzer();

		public static T FuzzType<T>()
		{
			return (T)FuzzType(typeof(T));
		}

		public static IEnumerable<T> FuzzType<T>(int count)
		{
			return Enumerable.Range(0, count).Select(x => FuzzType<T>());
		}

		public static object FuzzType(Type type)
		{
			object result;

			if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
			{
				var basicType = type.GetGenericArguments().Single();

				result = FuzzType(basicType);
			}
			else
			{
				if (type.IsAbstract || type.IsInterface)
				{
					var implementingTypes = type.Assembly.GetTypesThatImplement(type).ToList();

					if (implementingTypes.Any())
					{
						// TODO: pick one of these at random
						result = FuzzType(implementingTypes.First());
					}
					else
					{
						throw new ArgumentException(String.Format("Cannot fuzz type '{0}'. It is abstract.", type.Name));
					}
				}
				else
				{
					result = Fuzzer.Fuzz(type);
				}
			}

			return result;
		}
	}
}
