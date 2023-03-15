using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class TransactionExcept
    {
        public int Id { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
        public int Day { get; set; }

        [Required]
        public string ServicePackage { get; set; }

        public bool EndTime { get; set; }

        [Required]
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
