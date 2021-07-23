using BankingApp.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Entities.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
