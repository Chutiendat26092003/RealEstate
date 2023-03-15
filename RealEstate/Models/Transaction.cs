using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public char Status { get; set; } // cộng hoặc trừ
        public double Amount { get; set; } // số tiền

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        //public ICollection<TransactionExcept>? TransactionExcepts { get; set; }
    }
}
