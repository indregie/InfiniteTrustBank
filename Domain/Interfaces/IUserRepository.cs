using Domain.Dtos.Request;
using Domain.Dtos.Response;
using Domain.Entities;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<UserEntity?> CreateUserAsync(UserEntity user);
    Task<IEnumerable<UserEntity>> GetUsersAsync();
    Task<UserEntity?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<AccountEntity?>> GetUserAccountsAsync(Guid id);
}