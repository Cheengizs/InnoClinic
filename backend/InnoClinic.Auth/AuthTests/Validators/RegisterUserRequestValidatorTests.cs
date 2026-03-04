using AutoFixture;
using FluentAssertions;
using Presentation.Contracts;
using Presentation.Validators;

namespace AuthTests.Validators;

public class RegisterUserRequestValidatorTests
{
    private readonly RegisterUserRequestValidator _validator;
    private readonly IFixture _fixture;

    public RegisterUserRequestValidatorTests()
    {
        _validator = new RegisterUserRequestValidator();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Validate_WhenRequestIsValid_ShouldNotHaveErrors()
    {
        // Arrange
        var request = new RegisterUserRequest(
            PhoneNumber: "123456789", 
            Email: "test@example.com", 
            Password: "Password123"
        );

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]          
    [InlineData("not-email")] 
    [InlineData("tooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo_long_email@gmail.com")] 
    
    public async Task Validate_WhenEmailIsInvalid_ShouldHaveError(string invalidEmail)
    {
        // Arrange
        var request = _fixture.Build<RegisterUserRequest>()
            .With(x => x.Email, invalidEmail)
            .Create();

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("p")]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    [InlineData("without_upper")]
    [InlineData("WITHOUT_LOWER")]
    [InlineData("without_number")]
    public async Task Validate_WhenPasswordIsInvalid_ShouldHaveError(string psw)
    {
        // Arrange
        var request = _fixture.Build<RegisterUserRequest>()
            .With(x => x.Password, psw) 
            .Create();

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();   
    }
}
