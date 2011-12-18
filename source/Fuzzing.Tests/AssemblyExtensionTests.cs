using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Fuzzing.Tests.TestTypes;

using NUnit.Framework;

namespace Fuzzing.Tests
{
	[TestFixture]
	public class AssemblyExtensionTests
	{
		private Assembly _assembly;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			_assembly = GetType().Assembly;
		}

		[Test]
		public void GetTypesThatImplement_ITestInterface_ReturnsBothImplementations()
		{
			var implementations = _assembly.GetTypesThatImplement<ITestInterface>();

			Assert.That(implementations, Is.Not.Empty);
			Assert.That(implementations.Count(), Is.EqualTo(2));
			Assert.That(implementations, Contains.Item(typeof(ConcreteTestTypeOne)));
			Assert.That(implementations, Contains.Item(typeof(ConcreteTestTypeTwo)));
		}

		[Test]
		public void GetTypesThatImplement_ITestGenericInterface_ReturnsGenericAndNonGenericImplementations()
		{
			var implementations = _assembly.GetTypesThatImplement<ITestGenericInterface<string>>();

			Assert.That(implementations, Is.Not.Empty);
			Assert.That(implementations.Count(), Is.EqualTo(2));
			Assert.That(implementations, Contains.Item(typeof(TestClassImplementsGenericInterface)));
			Assert.That(implementations, Contains.Item(typeof(TestGenericClassImplementsGenericInterface<>)));
		}
	}
}
