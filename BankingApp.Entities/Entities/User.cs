﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BankingApp.Entities.Entities
{
    public class User : IdentityUser<int>
    {
        public string RefreshToken { get; set; }
        public virtual IList<Deposit> Deposits { get; set; }
    }
}