namespace IsPrimeNumber.Infrastructure.Services;

public class NumberService : INumberService
{
    public bool IsPrimeNumber(string stringNumber)
    {
        var isNumber = int.TryParse(stringNumber, out var number);

        if (!isNumber) 
        { 
            throw new ArgumentException($"Parameter {stringNumber} is not valid. Parameter should be in numeric format.");
        }
        
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }
}
