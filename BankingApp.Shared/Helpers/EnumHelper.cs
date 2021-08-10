using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BankingApp.Shared.Extensions;

namespace BankingApp.Shared.Helpers
{
    /// <summary>
    /// Enum helper methods.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Parses enum instance from its text presentation.
        /// </summary>
        /// <typeparam name="T">Type of enum to be parsed.</typeparam>
        /// <param name="value">Text presentation of parsed enum.</param>
        /// <returns>Instance of enum.</returns>
        public static T Parse<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Gets list of display names for specified enum type.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>List of display names.</returns>
        public static IList<string> GetDisplayValues<T>() where T : Enum
        {
            return GetNames<T>().Select(obj => Parse<T>(obj).GetDisplayValue()).ToList();
        }

        /// <summary>
        /// Gets list of enum field names.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>List of enum names.</returns>
        public static IList<string> GetNames<T>() where T : Enum
        {
            return typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        /// <summary>
        /// Gets list of enum instances fro specified type.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>List of enum instances.</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
