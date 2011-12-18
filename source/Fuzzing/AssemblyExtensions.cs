using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fuzzing
{
	public static class AssemblyExtensions
	{
		public static IEnumerable<Type> GetTypesThatImplement<T>(this Assembly assembly)
		{
			var type = typeof (T);

			return GetTypesThatImplement(assembly, type);
		}

		public static IEnumerable<Type> GetTypesThatImplement(this Assembly assembly, Type type)
		{
			IEnumerable<Type> result;

			var concreteTypes = assembly.GetExportedTypes()
				.Where(x => (x.IsAbstract == false) && (x.IsInterface == false));

			if (type.IsGenericType)
			{
				result = concreteTypes
					.Where(x => type.IsAssignableFrom(x) || type.IsAssignableFromGenericTypeDefinition(x));
			}
			else
			{
				result = concreteTypes
					.Where(type.IsAssignableFrom);
			}

			return result;
		}
	}
}
