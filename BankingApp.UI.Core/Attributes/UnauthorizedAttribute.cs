using System;

namespace BankingApp.UI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class UnauthorizedAttribute : Attribute
    {
    }
}
