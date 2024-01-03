using Application.Services;
using Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _userService.Get());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _userService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUser user)
    {
        var userCreated = await _userService.Create(user);
        return CreatedAtAction("Get", new { id = userCreated.Id, name = userCreated.Name });
    }

    [HttpGet("{id}/accounts")]
    public async Task<IActionResult> GetAccounts(Guid id)
    {
        return Ok(await _userService.GetUserAccounts(id));
    }

}
