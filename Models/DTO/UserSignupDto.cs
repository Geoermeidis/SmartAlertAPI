using System.ComponentModel.DataAnnotations;

namespace SmartAlertAPI.Models.Dto;

public class UserSignupDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}