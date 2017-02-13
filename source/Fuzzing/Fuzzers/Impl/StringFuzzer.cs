using System;
using System.Linq;

namespace Fuzzing.Fuzzers.Impl
{
	public class StringFuzzer : TypeFuzzer<string>
	{
		public override string Fuzz()
		{
			var length = GetRandomLength();

			var characters = Enumerable.Range(0, length).Select(x => GetRandomCharacter());

			var result = String.Join(String.Empty, characters);

			return result;
		}

		private int GetRandomLength()
		{
			var lengthData = new byte[1];
			RandomNumberGenerator.GetBytes(lengthData);

			return lengthData[0];
		}

		private char GetRandomCharacter()
		{
			var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-=!@#$%^&*()_+[]\\{}|;':\",./<>?`~".ToCharArray();

			var lengthData = new byte[1];
			RandomNumberGenerator.GetBytes(lengthData);

			var i = lengthData[0] % alphabet.Length;

			return alphabet[i];
		}
	}
}
