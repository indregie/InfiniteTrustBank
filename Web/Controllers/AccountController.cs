using Application.Services;
using Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{
    private AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _accountService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateAccount account)
    {
        var accountCreated = await _accountService.Create(account);
        return CreatedAtAction("Post", new { id = accountCreated.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _accountService.Delete(id);
        return NoContent();
    }
}
