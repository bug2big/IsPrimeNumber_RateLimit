namespace IsPrimeNumber.Infrastructure.Persistence.Entities;

public record User
{
    public string IPAddress { get; set; } = null!;
    public string Token { get; set; } = null!;
    public bool IsBlocked { get; set; }
    public int Attemps { get; set; }
    public DateTime SessionStartTimeOn { get; set; }
    public DateTime? BlockedTill { get; set;}
}
