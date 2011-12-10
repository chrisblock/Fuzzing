using System;
using System.Linq;

using NUnit.Framework;

namespace Fuzzing.Tests
{
	[TestFixture]
	public class ObjectFuzzerTests
	{
		[Test]
		public void FuzzType_string_IsNotNull()
		{
			var obj = ObjectFuzzer.FuzzType<string>();

			Assert.That(obj, Is.Not.Null);
		}

		[Test]
		public void CanFuzzAllSystemTypes()
		{
			var assembly = typeof (string).Assembly;

			var canFuzzAllSystemValueTypes = assembly.GetExportedTypes()
				.Where(x => x.IsPrimitive)
				.Select(x => new {type = x, fuzzable = ObjectFuzzer.CanFuzzType(x)})
				.Aggregate(true, (seed, value) =>
				{
					if (value.fuzzable == false)
					{
						Console.WriteLine(value.type.Name);
					}

					return seed && value.fuzzable;
				});

			Assert.That(canFuzzAllSystemValueTypes, Is.True);
		}
	}
}
