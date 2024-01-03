using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface IAccountRepository
    {
        Task<AccountEntity?> CreateAccountAsync(AccountEntity account);
        Task<AccountEntity?> GetAccountByIdAsync(Guid id);
    }
}