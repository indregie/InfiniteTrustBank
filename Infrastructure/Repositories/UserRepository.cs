using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid id)
    {
        string sql = "SELECT * FROM public.users WHERE id = @Id AND \"is_deleted\"=false ";
        var queryObject = new
        {
            Id = id
        };
        return await _connection.QueryFirstOrDefaultAsync<UserEntity?>(sql, queryObject);
    }

    public async Task<IEnumerable<UserEntity>> GetUsersAsync()
    {
        string sql = "SELECT * FROM public.users WHERE \"is_deleted\" = false";
        return await _connection.QueryAsync<UserEntity>(sql);
    }

    public async Task<UserEntity?> CreateUserAsync(UserEntity user)
    {
        string sql = "INSERT INTO public.users (name, address) VALUES (@Name, @Address) RETURNING id as Id, name as Name, address as Address";
        var queryObject = new
        {
            name = user.Name,
            address = user.Address
        };
        return await _connection.QueryFirstOrDefaultAsync<UserEntity?>(sql, queryObject);
    }

    public async Task<IEnumerable<AccountEntity?>> GetUserAccountsAsync(Guid id)
    {
        string sql = "SELECT id as Id, balance as Balance, type_id as TypeId, user_id as UserId FROM public.accounts WHERE user_id = @Id AND is_deleted = false";
        var queryObject = new
        {
            Id = id
        };
        var result = await _connection.QueryAsync<AccountEntity?>(sql, queryObject);
        return result;
    }
}
