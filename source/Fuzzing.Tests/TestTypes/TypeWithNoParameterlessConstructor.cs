namespace Fuzzing.Tests.TestTypes
{
	public class TypeWithNoParameterlessConstructor
	{
		public string Arugment { get; private set; }

		public TypeWithNoParameterlessConstructor(string arugment)
		{
			Arugment = arugment;
		}
	}
}
