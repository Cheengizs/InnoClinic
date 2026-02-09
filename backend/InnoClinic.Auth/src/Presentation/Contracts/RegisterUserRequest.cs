using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts;

public record RegisterUserRequest(
    [Required] [EmailAddress] string Email,
    [Required] string Password,
    string? PhoneNumber
);