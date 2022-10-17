using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Tests.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Kinetq.Validation.Tests.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IValidatorFactory _validatorFactory;

    public UserController(IValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    [HttpPost]
    public async Task<UserDto> Post(UserDto dto)
    {
        await _validatorFactory.Validate(dto);
        return dto;
    }
}