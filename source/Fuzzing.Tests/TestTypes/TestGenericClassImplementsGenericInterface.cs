namespace Fuzzing.Tests.TestTypes
{
	public class TestGenericClassImplementsGenericInterface<T> : ITestGenericInterface<T>
	{
		public T Property { get; set; }
	}
}
