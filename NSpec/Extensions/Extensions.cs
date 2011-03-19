using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSpec.Extensions
{
    public static class Extensions
    {
        public static T Instance<T>(this Type type) where T : class
        {
            return type.GetConstructors()[0].Invoke(new object[0]) as T;
        }

        public static IEnumerable<MethodInfo> Methods(this Type type, IEnumerable<string> exclusions=null)
        {
            return type.GetMethods().Where(m => exclusions == null ||  !exclusions.Contains(m.Name));
        }

        public static string Times(this string source, int times)
        {
            if (times == 0) return "";

            var s = "";

            for (int i = 0; i < times; i++)
                s += source;

            return s;
        }

        public static void Print(this object o)
        {
            Console.WriteLine(o.ToString());
        }

        /// <summary>
        /// Action(T) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be executed across a common set of examples.
        /// </summary>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var t in source)
                action(t);

            return source;
        }

        /// <summary>
        /// Action(T, T) will get executed for each consecutive 2 items in a list.  You can use this to specify a suite of data that needs to be executed across a common set of examples.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> EachConsecutive2<T>(this IEnumerable<T> source, Action<T, T> action)
        {
            var array = source.ToArray();
            for (int i = 0; i < array.Length - 1; i++)
            {
                action(array[i], array[i + 1]);
            }

            return source;
        }

        /// <summary>
        /// Action(T, U) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        public static void Do<T, U>(this Tuples<T, U> source, Action<T, U> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2);
        }

        /// <summary>
        /// Action(T, U) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        public static void Do<T, U>(this Dictionary<T, U> source, Action<T, U> action)
        {
            foreach (var kvp in source)
                action(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Action(T, U, V) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        public static void Do<T, U, V>(this Tuples<T, U, V> source, Action<T, U, V> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2, tup.Item3);
        }

        /// <summary>
        /// Action(T, U, V, W) will get executed for each item in the list.  You can use this to specify a suite of data that needs to be execute across a common set of examples.
        /// </summary>
        public static void Do<T, U, V, W>(this Tuples<T, U, V, W> source, Action<T, U, V, W> action)
        {
            foreach (var tup in source)
                action(tup.Item1, tup.Item2, tup.Item3, tup.Item4);
        }

        /// <summary>
        /// Extension method that wraps String.Format.
        /// <para>Usage: string result = "{0} {1}".With("hello", "world");</para>
        /// </summary>
        public static string With(this string source, params object[] objects)
        {
            var o = Sanitize(objects);
            return string.Format(source, o);
        }

        public static string[] Sanitize(this object[] source)
        {
            return source.ToList().Select(o =>
            {
                if (o.GetType().Equals(typeof(int[])))
                {
                    var s = "";

                    (o as int[]).Do(i => s += i + ",");

                    if (s == "")
                        return "[]";

                    return "[" + s.Remove(s.Length - 1, 1) + "]";
                }

                return o.ToString();
            }).ToArray();
        }
    }
}