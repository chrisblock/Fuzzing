using System;
using System.Linq;

namespace Fuzzing
{
	public static class TypeExtensions
	{
		public static bool IsAssignableFromGenericTypeDefinition(this Type type, Type genericTypeDefinition)
		{
			var result = false;

			if (genericTypeDefinition.IsGenericTypeDefinition)
			{
				var typeParameters = genericTypeDefinition.GetGenericArguments();
				var typeArguments = type.GetGenericArguments();

				if (typeParameters.Length == typeArguments.Length)
				{
					var genericParameterConstraintsAreSatisfied = typeParameters
						.Zip(typeArguments, MatchesGenericParameterConstraints)
						.All(x => x);

					if (genericParameterConstraintsAreSatisfied)
					{
						var otherType = genericTypeDefinition.MakeGenericType(typeArguments);

						result = type.IsAssignableFrom(otherType);
					}
				}
			}

			return result;
		}

		private static bool MatchesGenericParameterConstraints(Type typeParameter, Type typeArgument)
		{
			return typeParameter.GetGenericParameterConstraints().All(typeArgument.IsAssignableFrom);
		}
	}
}
