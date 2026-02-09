using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Account;

public record LoginUserRequest (
    [Required] [EmailAddress] string Email,
    [Required] string Password
);