using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BankingApp.Shared.Extensions
{
    /// <summary>
    /// Custom enum extensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Parses dispaly name of enum.
        /// </summary>
        /// <param name="value">Enum instance.</param>
        /// <returns>Display name attached to enum item with <see cref="DisplayAttribute"/>.</returns>
        public static string GetDisplayValue(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
            { 
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
            }

            if (descriptionAttributes == null)
            {
                return string.Empty;
            }

            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string),
                new Type[0], null);

            if (resourceKeyProperty != null)
            {
                return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
            }

            return resourceKey;
        }
    }
}
