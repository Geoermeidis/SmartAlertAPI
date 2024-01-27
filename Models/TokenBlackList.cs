using System.ComponentModel.DataAnnotations;

namespace SmartAlertAPI.Models
{
    public class TokenBlackList
    {
        [Key]
        public string Token { get; set; } 
    }
}
