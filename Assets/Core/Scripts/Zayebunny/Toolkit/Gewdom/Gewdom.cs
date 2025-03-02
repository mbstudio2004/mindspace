using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Nocci.Zayebunny.Gewdom
{
    public static class Gewdom
    {
        private static Random _random;

        public static string GetSeed { get; private set; } = "Gewad";


        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // ReSharper disable once ParameterHidesMember
        public static void SetSeed(string seed)
        {
            GetSeed = seed;
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(GetSeed)));
            _random = new Random(hash);
        }

        public static void SetRandomSeed()
        {
            SetSeed(RandomString(16));
        }

        public static void ResetGewdom()
        {
            SetSeed(GetSeed);
        }


        public static double RandomValue()
        {
            if (_random == null) SetRandomSeed();

            return _random.NextDouble();
        }

        public static int RandomRange(int a, int b)
        {
            if (_random == null) SetRandomSeed();

            return _random.Next(a, b);
        }

        public static double RandomRange(double a, double b)
        {
            return RandomValue() * (b - a) + a;
        }

        public static float RandomRange(float a, float b)
        {
            return (float)RandomValue() * (b - a) + a;
        }

        public static double URandomValue()
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.NextDouble();
        }

        public static double URandomRange(int a, int b)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(a, b);
        }

        public static double URandomRange(double a, double b)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            return URandomValue() * (b - a) + a;
        }


        /// <summary>
        ///     Swaps two items in a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void GewSwap<T>(this IList<T> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }

        /// <summary>
        ///     Shuffles a list randomly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void GewShuffleList<T>(this IList<T> list)
        {
            for (var i = 0; i < list.Count; i++) list.GewSwap(i, RandomRange(i, list.Count));
        }

        public static T RandomElement<T>(IList<T> list)
        {
            return list[RandomRange(0, list.Count)];
        }
    }
}