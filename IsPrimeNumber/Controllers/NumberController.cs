using IsPrimeNumber.Infrastructure.Services;
using IsPrimeNumber.Models;
using Microsoft.AspNetCore.Mvc;

namespace IsPrimeNumber.Controllers;

[ApiController]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class NumberController : ControllerBase
{
    private readonly INumberService _numberService;

    public NumberController(INumberService numberService)
    {
        _numberService = numberService;
    }

    [HttpGet]
    [Route("isPrimeNumber")]
    public IActionResult IsPrimeNumber([FromQuery] IsPrimeNumberModel model)
    {
        try
        {
            var isPrimeNumber = _numberService.IsPrimeNumber(model.Number);
            var response = new IsPrimeNumberResponse
            {
                HasError = false,
                IsPrime = isPrimeNumber,
                Number = model.Number
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            var response = new IsPrimeNumberResponse
            {
                HasError = true,
                ErrorMessage = ex.Message
            };

            return BadRequest(response);
        }
    }
}
