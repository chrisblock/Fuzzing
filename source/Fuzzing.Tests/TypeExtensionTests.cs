using System.Collections.Generic;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Fuzzing.Tests
{
	[TestFixture]
	public class TypeExtensionTests
	{
		[Test]
		public void IsAssignableFromGenericTypeDefinition_DerivedGenericTypeDefinition_IsTrue()
		{
			var type = typeof(IList<string>);

			Assert.That(type.IsAssignableFromGenericTypeDefinition(typeof(IEnumerable<>)), Is.False);
		}

		[Test]
		public void IsAssignableFromGenericTypeDefinition_ConcreteGenericTypeDefinition_IsTrue()
		{
			var type = typeof(IEnumerable<string>);

			Assert.That(type.IsAssignableFromGenericTypeDefinition(typeof(List<>)), Is.True);
		}

		[Test]
		public void IsAssignableFromGenericTypeDefinition_InterfaceGenericTypeDefinition_IsTrue()
		{
			var type = typeof(IEnumerable<string>);

			Assert.That(type.IsAssignableFromGenericTypeDefinition(typeof(IList<>)), Is.True);
		}
	}
}
