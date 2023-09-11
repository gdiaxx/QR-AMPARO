using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QrAmparoApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [NotMapped] 
        public string PasswordString{ get ; set ;}
    }
}