using System;

namespace Fuzzing.Fuzzers.Impl
{
	public class CharacterFuzzer : TypeFuzzer<char>
	{
		private const int CharacterSize = sizeof (char);

		public override char Fuzz()
		{
			var bytes = new byte[CharacterSize];

			RandomNumberGenerator.GetBytes(bytes);

			var character = BitConverter.ToChar(bytes, 0);

			return character;
		}
	}
}
