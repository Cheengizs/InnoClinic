namespace Application.Dto.Account;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);