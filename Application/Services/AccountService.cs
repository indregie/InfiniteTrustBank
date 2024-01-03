using Domain.Dtos.Request;
using Domain.Dtos.Response;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;

namespace Application.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly UserService _userService;
    public AccountService(IAccountRepository accountRepository, UserService userService)
    {
        _accountRepository = accountRepository;
        _userService = userService;
    }

    public async Task<Account> GetById(Guid id)
    {
        AccountEntity accountEn = new AccountEntity()
        {
            Id = id
        };
        AccountEntity? result = await _accountRepository.GetAccountByIdAsync(id);
        if (result == null)
        {
            throw new AccountNotFoundException();
        }
        Account response = new Account()
        {
            Id = result.Id,
            Balance = result.Balance,
            TypeId = result.TypeId,
            UserId = result.UserId
        };
        return response;
    }

    public async Task<Account> Create(CreateAccount account)
    {
        var numOfAccounts = await _userService.GetUserAccounts(account.UserId);

        if (numOfAccounts.Count() >= 2)
        {
            throw new AccountsLimitException();
        }

        AccountEntity accountEn = new AccountEntity()
        {
            TypeId = account.TypeId,
            UserId = account.UserId
        };

        AccountEntity result = await _accountRepository.CreateAccountAsync(accountEn) ?? throw new Exception();
        Account response = new Account()
        {
            Id = result.Id,
            Balance = result.Balance,
            TypeId = result.TypeId,
            UserId = result.UserId
        };

        return response;
    }

    public async Task Delete(Guid id)
    {
        if (await GetById(id) == null)
        {
            throw new AccountNotFoundException();
        }
        await _accountRepository.DeleteAsync(id);
    }
}
