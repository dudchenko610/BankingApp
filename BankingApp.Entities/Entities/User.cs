using BankingApp.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BankingApp.Entities.Entities
{
    public class User : IdentityUser<int>, IBaseEntity
    {
        public virtual IList<Deposit> Deposits { get; set; }
        public bool IsBlocked { get; set; }
    }
}
