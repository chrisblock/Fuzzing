﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace Fuzzing
{
	public static class ObjectFuzzer
	{
		private static readonly char[] AvailableCharacters = "abcdefghijklmnopqrstuvwxyz ".ToCharArray();

		private static readonly IDictionary<Type, Func<int, object>> TypeFuzzerDictionary = new ConcurrentDictionary<Type, Func<int, object>>();

		static ObjectFuzzer()
		{
			RegisterTypeFuzzer(seed => (sbyte)GetNextRandomBytes(seed, 1).First());
			RegisterTypeFuzzer(seed => GetNextRandomBytes(seed, 1).First());
			RegisterTypeFuzzer(seed => BitConverter.ToInt16(GetNextRandomBytes(seed, 2), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToInt32(GetNextRandomBytes(seed, 4), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToInt64(GetNextRandomBytes(seed, 8), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToUInt16(GetNextRandomBytes(seed, 2), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToUInt32(GetNextRandomBytes(seed, 4), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToUInt64(GetNextRandomBytes(seed, 8), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToSingle(GetNextRandomBytes(seed, 8), 0));
			RegisterTypeFuzzer(seed => BitConverter.ToDouble(GetNextRandomBytes(seed, 8), 0));
			RegisterTypeFuzzer(seed =>
			{
				var ints = new int[3];
				var bytes = GetNextRandomBytes(seed, 14);

				for (var i = 0; i < ints.Length; i++)
				{
					ints[i] = BitConverter.ToInt32(bytes, sizeof(int) * i);
				}

				return new Decimal(ints[0], ints[1], ints[2], BitConverter.ToBoolean(bytes, 12), (byte)(bytes[13] % 29));
			});
			RegisterTypeFuzzer(seed =>
			{
				var random = new Random(seed);
				var buffer = new byte[random.Next(Math.Abs(seed % 500))];
				random.NextBytes(buffer);
				return String.Join(String.Empty, buffer.Select(x => AvailableCharacters[x % AvailableCharacters.Length]));
			});
			RegisterTypeFuzzer(seed => Guid.NewGuid());
			RegisterTypeFuzzer(seed => DateTime.Now.Add(TimeSpan.FromSeconds(seed)));
		}

		private static byte[] GetNextRandomBytes(int seed, int number)
		{
			var random = new Random(seed);
			var buf = new byte[number];

			random.NextBytes(buf);

			return buf;
		}

		private static readonly object RngLocker = new object();
		private static RandomNumberGenerator _randomNumberGenerator;
		private static RandomNumberGenerator RandomNumberGenerator
		{
			get
			{
				if (_randomNumberGenerator == null)
				{
					lock (RngLocker)
					{
						if (_randomNumberGenerator == null)
						{
							var rng = new RNGCryptoServiceProvider();
							Thread.MemoryBarrier();
							_randomNumberGenerator = rng;
						}
					}
				}

				return _randomNumberGenerator;
			}
		}

		private static int GetSeed()
		{
			var integerBytes = new byte[4];
			RandomNumberGenerator.GetBytes(integerBytes);

			return BitConverter.ToInt32(integerBytes, 0);
		}

		public static void RegisterTypeFuzzer<T>(Func<int, T> createType, bool overwrite = false)
		{
			var type = typeof(T);

			if ((TypeFuzzerDictionary.ContainsKey(type) == false) || (overwrite == true))
			{
				lock (((IDictionary)TypeFuzzerDictionary).SyncRoot)
				{
					if ((TypeFuzzerDictionary.ContainsKey(type) == false) || (overwrite == true))
					{
						TypeFuzzerDictionary[type] = x => createType(x);
					}
				}
			}
		}

		public static bool CanFuzzType<T>()
		{
			var type = typeof(T);

			return CanFuzzType(type);
		}

		public static bool CanFuzzType(Type type)
		{
			return TypeFuzzerDictionary.ContainsKey(type);
		}

		public static T FuzzType<T>()
		{
			return (T)FuzzType(typeof(T));
		}

		public static IEnumerable<T> FuzzType<T>(int count = 1000)
		{
			return Enumerable.Range(0, count).Select(x => FuzzType<T>());
		}

		public static object FuzzType(Type type)
		{
			object result;

			if (CanFuzzType(type))
			{
				var seed = GetSeed();

				result = TypeFuzzerDictionary[type](seed);
			}
			else if (type.IsEnum)
			{
				result = FuzzEnum(type);
			}
			else if (type.IsAbstract)
			{
				throw new ArgumentException(String.Format("Cannot fuzz type '{0}'. It is abstract.", type.Name));
			}
			else if (type.IsInterface)
			{
				throw new ArgumentException(String.Format("Cannot fuzz type '{0}'. It is an interface.", type.Name));
			}
			else if (type.IsValueType || type.IsPrimitive)
			{
				throw new ArgumentException(String.Format("Cannot fuzz type '{0}'. It is unrecognized.", type.Name));
			}
			else
			{
				result = ReflectiveFuzzType(type);
			}

			return result;
		}

		private static object FuzzEnum(Type enumType)
		{
			var values = enumType.GetEnumValues();

			var random = new Random(GetSeed());

			var index = random.Next(values.Length);

			return values.GetValue(index);
		}

		private static object ReflectiveFuzzType(Type type)
		{
			if (type.GetConstructors().Where(x => x.GetParameters().Length == 0).Any() == false)
			{
				throw new ArgumentException(String.Format("Cannot fuzz type '{0}'. It has no public parameterless constructors.", type.Name));
			}

			var instance = Activator.CreateInstance(type);

			var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var property in properties)
			{
				if (property.PropertyType.IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
				{
					var actualType = property.PropertyType.GetGenericArguments().Single();

					property.GetSetMethod(true).Invoke(instance, new[] { FuzzType(actualType) });
				}
				else
				{
					property.GetSetMethod(true).Invoke(instance, new[] { FuzzType(property.PropertyType) });
				}
			}

			return instance;
		}
	}
}