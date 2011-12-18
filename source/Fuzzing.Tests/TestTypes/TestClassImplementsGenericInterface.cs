namespace Fuzzing.Tests.TestTypes
{
	public class TestClassImplementsGenericInterface : ITestGenericInterface<string>
	{
		public string Property { get; set; }
	}
}
