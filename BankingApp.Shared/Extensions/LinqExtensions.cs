using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.Shared.Extensions
{
    /// <summary>
    /// Custom linq extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Subtracts from IEnumerable A IEnumerable B using as a feature to compare specified property of A and B./>
        /// </summary>
        /// <typeparam name="TA">Type of elements of IEnumerable A.</typeparam>
        /// <typeparam name="TB">Type of elements of IEnumerable B.</typeparam>
        /// <typeparam name="TK">Type of choosen properties. Chosen properties must have the same type.</typeparam>
        /// <param name="a">IEnumerable A.</param>
        /// <param name="b">IEnumerable B.</param>
        /// <param name="selectKeyA">Function that specifies feature-property for IEnumerable A.</param>
        /// <param name="selectKeyB">Function that specifies feature-property for IEnumerable B.</param>
        /// <returns>Returns difference of two sets of data.</returns>
        public static IEnumerable<TA> Except<TA, TB, TK>(this IEnumerable<TA> a, IEnumerable<TB> b, Func<TA, TK> selectKeyA, Func<TB, TK> selectKeyB)
        {
            return a.Where(aItem => !b.Select(bItem => selectKeyB(bItem)).Contains(selectKeyA(aItem), null));
        }
    }
}
