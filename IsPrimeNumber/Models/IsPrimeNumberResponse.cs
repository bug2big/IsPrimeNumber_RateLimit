namespace IsPrimeNumber.Models;

public record IsPrimeNumberResponse
{
    public bool HasError { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Number { get; set; }
    public bool? IsPrime { get; set; }
}
