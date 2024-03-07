using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUtils
{
    public static class LINQExtensions
    {
        public static IEnumerable<T> SkipExceptions<T>(this IEnumerable<T> values, Action<Exception>? errorHandler = null)
        {
            return values.SkipExceptions<T, Exception>(errorHandler);
        }

        public static IEnumerable<T> SkipExceptions<T, TException>(this IEnumerable<T> values, Action<TException>? errorHandler)
            where TException : Exception
        {
            using (var enumerator = values.GetEnumerator())
            {
                bool next = true;
                while (next)
                {
                    try
                    {
                        next = enumerator.MoveNext();
                    }
                    catch (TException ex)
                    {
                        errorHandler?.Invoke(ex);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    if (next) yield return enumerator.Current;
                }
            }
        }

        public static ParallelQuery<T> SkipExceptions<T>(this ParallelQuery<T> values, Action<Exception>? errorHandler = null)
        {
            return SkipExceptions<T,Exception>(values, errorHandler);
        }

        public static ParallelQuery<T> SkipExceptions<T, TException>(this ParallelQuery<T> values, Action<TException>? errorHandler = null)
            where TException: Exception
        {
            return values.Select(x=>
            {
                try
                {
                    return new NullableObject<T>(x);
                }
                catch (TException ex)
                {
                    errorHandler?.Invoke(ex);
                    return new NullableObject<T>();
                }
                
            }).Where(x=>x.HasValue).Select(x=>x.Value!);
        }

        struct NullableObject<T>
        {
            public readonly T? Value;

            public bool HasValue => Value != null;

            public NullableObject(T value)
            {
                Value = value;
            }

            public NullableObject()
            {
            }
        }
    }



}
