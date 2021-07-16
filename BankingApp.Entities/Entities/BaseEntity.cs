using System.ComponentModel.DataAnnotations;

namespace BankingApp.Entities.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
