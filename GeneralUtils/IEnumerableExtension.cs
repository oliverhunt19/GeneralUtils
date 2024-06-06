using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUtils
{
    public static class IEnumerableExtension
    {

        public static IEnumerable<T> OfType<T>(this IEnumerable<T> source, Type type)
        {
            return OfTypeIterator(source, type);
        }

        private static IEnumerable<T> OfTypeIterator<T>(this IEnumerable<T> source, Type type)
        {
            foreach(T obj in source)
            {
                if(obj.GetType() == type)
                {
                    yield return obj;
                }
            }
        }

        /// <summary>
        /// This checks if the type <typeparamref name="T"/> is inherited by <paramref name="TopType"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TopType"></param>
        /// <returns></returns>
        /// <remarks>
        /// This could be used to check that a item in a list is of the type de
        /// </remarks>
        public static bool TypeInherits<T>(this Type TopType)
        {
            Type CheckType = typeof(T);
            return TypeInherits(CheckType, TopType);
        }

        /// <summary>
        /// This check to see if the top type inherits the checktype
        /// </summary>
        /// <param name="CheckType"></param>
        /// <param name="TopType"></param>
        /// <returns></returns>
        public static bool TypeInherits(Type CheckType, Type TopType)
        {
            return TopType.IsAssignableTo(CheckType);
            if (CheckType.IsInterface)
            {

            }
            if(TopType?.BaseType is null)
            {
                return false;
            }
            else if(TopType.BaseType == CheckType)
            {
                return true;
            }
            else
            {
                return TypeInherits(CheckType,TopType.BaseType);
            }
        }
    }
}
