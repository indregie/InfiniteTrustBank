using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IDbConnection _connection;

    public AccountRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<AccountEntity?> GetAccountByIdAsync(Guid id)
    {
        string sql = "SELECT id as Id, balance as Balance, type_id as TypeId, user_id as UserId " +
            "FROM public.accounts WHERE id = @Id AND is_deleted=false";
        var queryObject = new
        {
            Id = id
        };
        return await _connection.QueryFirstOrDefaultAsync<AccountEntity?>(sql, queryObject);
    }

    public async Task<AccountEntity?> CreateAccountAsync(AccountEntity account)
    {
        string sql = "INSERT INTO public.accounts (type_id, user_id) VALUES (@TypeId, @UserId) " +
            "RETURNING id as Id, type_id as TypeId, user_id as UserId";
        var queryObject = new
        {
            TypeId = account.TypeId,
            UserId = account.UserId
        };
        return await _connection.QueryFirstOrDefaultAsync<AccountEntity?>(sql, queryObject);
    }

    public async Task DeleteAsync(Guid id)
    {
        string sql = "UPDATE public.accounts SET is_deleted = true WHERE id=@Id";
        var queryObject = new
        {
            Id = id
        };

        await _connection.ExecuteAsync(sql, queryObject);
    }
}
