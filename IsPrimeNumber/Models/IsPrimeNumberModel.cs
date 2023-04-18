using System.ComponentModel.DataAnnotations;

namespace IsPrimeNumber.Models;

public record IsPrimeNumberModel
{
    public string IPAddress { get; set; } = null!;

    [MaxLength(DefaultValues.MaxNumberLength)]
    public string Number { get; set; } = null!;

    [MaxLength(DefaultValues.MaxTokenLength)]
    public string Token { get; set; } = null!;
}
