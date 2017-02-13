using Fuzzing.Fuzzers;
using Fuzzing.Fuzzers.Impl;

using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace Fuzzing.Tests.Fuzzers
{
	[TestFixture]
	public class StringFuzzerTests
	{
		private IFuzzer _fuzzer;

		[SetUp]
		public void TestSetUp()
		{
			_fuzzer = new StringFuzzer();
		}

		[Test]
		[Repeat(1000)]
		public void Fuzz_ReturnsNonNullObject()
		{
			var randomString = _fuzzer.Fuzz(typeof (string));

			Assert.That(randomString, Is.Not.Null);
		}
	}
}
