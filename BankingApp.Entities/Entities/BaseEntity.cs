using BankingApp.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Entities.Entities
{
    /// <summary>
    /// Represents base entity
    /// </summary>
    public class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
