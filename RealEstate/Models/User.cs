using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string? Address { get; set; }

        [Required]
        public double Balance { get; set; }

        public string? Image { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        [Required(ErrorMessage = "Please choose Front image")]
        [NotMapped]
        public IFormFile FrontImage { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<TransactionExcept>? TransactionExcepts { get; set; }

    }
}
