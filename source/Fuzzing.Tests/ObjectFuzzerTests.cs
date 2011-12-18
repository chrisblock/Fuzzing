using System;
using System.Linq;

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
		public void HasExplicitFuzzingStrategyBeenDefinedForType_AllNonPointerSystemPrimitives_IsTrue()
		{
			var assembly = typeof (string).Assembly;

			var systemPrimitivesThatCantBeFuzzed = assembly.GetExportedTypes()
				.Where(x => x.IsPrimitive)
				.Where(x => (x != typeof(IntPtr)) && (x != typeof(UIntPtr))) // i don't care about these types. they're scary.
				.Where(ObjectFuzzer.IsExplicitFuzzingStrategyDefinedForType);

			Assert.That(systemPrimitivesThatCantBeFuzzed, Is.Empty);
		}

		[Test]
		public void HasExplicitFuzzingStrategyBeenDefinedForType_String_IsTrue()
		{
			var result = ObjectFuzzer.IsExplicitFuzzingStrategyDefinedForType<string>();

			Assert.That(result, Is.True);
		}
	}
}
