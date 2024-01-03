using Domain.Dtos.Request;
using Domain.Dtos.Response;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> Get()
    {
        IEnumerable<UserEntity> users = await _userRepository.GetUsersAsync();
        IEnumerable<User> responseUsers = users
            .Select(user => new User
            {
                Id = user.Id,
                Name = user.Name,
                Address = user.Address
            });
        return responseUsers;
    }

    public async Task<User> GetById(Guid id)
    {
        UserEntity userEn = new UserEntity()
        {
            Id = id
        };
        UserEntity? result = await _userRepository.GetUserByIdAsync(id);
        if (result == null)
        {
            throw new UserNotFoundException();
        }
        User response = new User()
        {
            Id = result.Id,
            Name = result.Name,
            Address = result.Address
        };
        return response;
    }

    public async Task<IEnumerable<Account>> GetUserAccounts(Guid id)
    {
        IEnumerable<AccountEntity?> userAccounts = await _userRepository.GetUserAccountsAsync(id);
        IEnumerable<Account> responseUserAccounts = userAccounts
            .Select(account => new Account
            {
                Id = account.Id,
                Balance = account.Balance,
                TypeId = account.TypeId,
                UserId = id
            });
        return responseUserAccounts;
    }

    public async Task<User> Create(CreateUser user)
    {
        UserEntity userEn = new UserEntity()
        {
            Name = user.Name,
            Address = user.Address
        };

        UserEntity result = await _userRepository.CreateUserAsync(userEn) ?? throw new Exception();
        User response = new User()
        {
            Id = result.Id,
            Name = result.Name,
            Address = result.Address
        };

        return response;
    }
}
