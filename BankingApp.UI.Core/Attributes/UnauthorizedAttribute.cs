using System;

namespace BankingApp.UI.Core.Attributes
{
    /// <summary>
    /// Used to mark blazor page that must be accessed only by unauthorized user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class UnauthorizedAttribute : Attribute
    {
    }
}
