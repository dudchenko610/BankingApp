namespace BankingApp.Entities.Interfaces
{
    /// <summary>
    /// Interface for all database entities 
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        public int Id { get; set; }
    }
}