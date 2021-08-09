using BankingApp.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BankingApp.Entities.Entities
{
    /// <summary>
    /// Represents entity of "AspNetUsers" table.
    /// </summary>
    public class User : IdentityUser<int>, IBaseEntity
    {
        /// <summary>
        /// List of <see cref="Deposit"/>s. User for data querying.
        /// </summary>
        public virtual IList<Deposit> Deposits { get; set; }

        /// <summary>
        /// Marks user as blocked (true value) and unblocked otherwise.
        /// </summary>
        public bool IsBlocked { get; set; }
    }
}
