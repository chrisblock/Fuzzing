using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fuzzing.Fuzzers.Impl
{
	public class ReflectionFuzzer : IFuzzer
	{
		private readonly IFuzzer _fuzzer;

		public ReflectionFuzzer(IFuzzer fuzzer)
		{
			_fuzzer = fuzzer;
		}

		public Type FuzzedType { get { return null; } }

		public object Fuzz(Type type)
		{
			if (type.GetConstructors().Any(x => x.GetParameters().Length == 0) == false)
			{
				// TODO: fuzz the inputs to the constructor (could be difficult given the tendency to use DI and IoC...)
				throw new ArgumentException(String.Format("Cannot fuzz type '{0}'. It has no public parameterless constructors.", type.Name));
			}

			var instance = Activator.CreateInstance(type);

			var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var property in properties)
			{
				var setMethod = property.GetSetMethod(true);

				if (setMethod != null)
				{
					var fuzzedPropertyValue = _fuzzer.Fuzz(property.PropertyType);

					setMethod.Invoke(instance, new[] { fuzzedPropertyValue });
				}
			}

			return instance;
		}

		public IEnumerable<object> Fuzz(Type type, int count)
		{
			return Enumerable.Range(0, count)
				.Select(x => Fuzz(type));
		}
	}
}
