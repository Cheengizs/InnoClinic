using System.ComponentModel.DataAnnotations;

namespace Presentation.Contracts;

public record LoginUserRequest (
    [Required] [EmailAddress] string Email,
    [Required] string Password
);