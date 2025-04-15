using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Black_Magic_Backend.Models {
    public class User {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; private set; }

        [Required]
        public string Salt { get; private set; }

        public void SetPassword(string password) {
            Salt = GenerateSalt();
            PasswordHash = HashPassword(password, Salt);
        }

        public bool ValidatePassword(string password) {
            string hashedInput = HashPassword(password, Salt);
            return hashedInput == PasswordHash;
        }

        private string GenerateSalt() {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt) {
            using (var sha256 = SHA256.Create()) {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
