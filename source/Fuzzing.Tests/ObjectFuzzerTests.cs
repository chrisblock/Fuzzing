// ReSharper disable InconsistentNaming

using System;

using Fuzzing.Tests.TestTypes;

using NUnit.Framework;

namespace Fuzzing.Tests
{
	[TestFixture]
	public class ObjectFuzzerTests
	{
		[Test]
		public void FuzzType_String_IsNotNull()
		{
			var result = ObjectFuzzer.FuzzType<string>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void FuzzType_NullableInteger_IsNotNull()
		{
			var result = ObjectFuzzer.FuzzType<int?>();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void FuzzType_ITestInterface_IsNotNull()
		{
			var result = ObjectFuzzer.FuzzType<ITestInterface>();

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Property, Is.Not.Null);
		}

		[Test]
		public void FuzzType_Enum_IsValidEnumValue()
		{
			var result = ObjectFuzzer.FuzzType<TestEnum>();

			Assert.That(Enum.GetValues(typeof(TestEnum)), Contains.Item(result));
		}

		[Test]
		public void FuzzType_TypeWithNoParameterlessConstructor_ThrowsArgumentException()
		{
			Assert.That(() => ObjectFuzzer.FuzzType<TypeWithNoParameterlessConstructor>(), Throws.ArgumentException);
		}

		[Test]
		public void FuzzType_IUnImplementedInterface_ThrowsArgumentException()
		{
			Assert.That(() => ObjectFuzzer.FuzzType<IUnImplementedInterface>(), Throws.ArgumentException);
		}
	}
}
