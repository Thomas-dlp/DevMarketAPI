using System.ComponentModel.DataAnnotations;

namespace DevMarketAPI.Models
{
    public class StudioCredentials
    {

        [Key]
        public Guid Id { get; private set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime DateCreated { get; private set; } 

        public StudioCredentials ( string email, string passwordHash)
        {
            Id = new Guid();
            Email = email;
            PasswordHash = passwordHash;
            DateCreated = DateTime.Now;
        }
    }
}
