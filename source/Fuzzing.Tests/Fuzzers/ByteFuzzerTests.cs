using Fuzzing.Fuzzers;
using Fuzzing.Fuzzers.Impl;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Fuzzing.Tests.Fuzzers
{
	[TestFixture]
	public class ByteFuzzerTests
	{
		private IFuzzer _fuzzer;

		[SetUp]
		public void TestSetUp()
		{
			_fuzzer = new ByteFuzzer();
		}

		[Test]
		[Repeat(1000)]
		public void Fuzz_ReturnsNonNullObject()
		{
			var randomByte = _fuzzer.Fuzz(typeof (byte));

			Assert.That(randomByte, Is.Not.Null);
		}
	}
}
