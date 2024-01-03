using Application.Services;
using Domain.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : Controller
{
    private TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{user_id}")]
    public async Task<IActionResult> Get(Guid user_id)
    {
        return Ok(await _transactionService.GetByUser(user_id));
    }

    [HttpPost("/Transfer")]
    public async Task<IActionResult> Transfer(CreateTransfer transaction)
    {
        var result = await _transactionService.CreateTransfer(transaction);
        return Ok(result);
    }

    [HttpPost("/Topup")]
    public async Task<IActionResult> Topup(CreateTopup transaction)
    {
        var result = await _transactionService.CreateTopup(transaction);
        return Ok(result);
    }
}