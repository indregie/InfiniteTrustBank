using Dapper;
using Domain.Entities;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbConnection _connection;

    public TransactionRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<TransactionEntity?> CreateTransferAsync(TransactionEntity transaction)
    {
        _connection.Open();
        using (var transactionSql = _connection.BeginTransaction(IsolationLevel.Snapshot))
        {
            {
                string sql = "UPDATE public.accounts SET balance=balance+@sum WHERE id = @receiverAccountId";
                var queryObject = new
                {
                    sum = transaction.Sum,
                    receiverAccountId = transaction.ReceiverAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = "UPDATE public.accounts SET balance=balance-@sum WHERE id = @senderAccountId";
                var queryObject = new
                {
                    sum = transaction.Sum,
                    senderAccountId = transaction.SenderAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = @"INSERT INTO public.transactions (transaction_type_id, sum, sender_acc_id, receiver_acc_id) 
                    VALUES (1, 1, @senderAccountId, @senderAccountId) ";
                var queryObject = new
                {
                    senderAccountId = transaction.SenderAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = "UPDATE public.accounts SET balance=balance-1 WHERE id = @senderAccountId";
                var queryObject = new
                {
                    senderAccountId = transaction.SenderAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = "INSERT INTO public.transactions (transaction_type_id, sum, sender_acc_id, receiver_acc_id) " +
                "VALUES (3, @sum, @senderAccountId, @receiverAccountId) " +
                "RETURNING id as Id, transaction_type_id as TransactionTypeId, sum as Sum, " +
                "sender_acc_id as SenderAccountId, receiver_acc_id as ReceiverAccountId";
                var queryObject = new
                {
                    sum = transaction.Sum,
                    senderAccountId = transaction.SenderAccountId,
                    receiverAccountId = transaction.ReceiverAccountId
                };

                var result = await _connection.QueryFirstOrDefaultAsync<TransactionEntity>(sql, queryObject, transaction: transactionSql);
                transactionSql.Commit();
                return result;
            }
        }
    }

    public async Task<TransactionEntity?> CreateTopupAsync(TransactionEntity transaction)
    {
        _connection.Open();
        using (var transactionSql = _connection.BeginTransaction(IsolationLevel.Snapshot))
        {
            {
                string sql = "UPDATE public.accounts SET balance= balance + @sum WHERE id = @senderAccountId";
                var queryObject = new
                {
                    sum = transaction.Sum,
                    senderAccountId = transaction.SenderAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = "INSERT INTO public.transactions (transaction_type_id, sum, sender_acc_id, receiver_acc_id) " +
                "VALUES (2, @sum, @senderAccountId, @receiverAccountId) " +
                "RETURNING id as Id, transaction_type_id as TransactionTypeId, sum as Sum, " +
                "sender_acc_id as SenderAccountId, receiver_acc_id as ReceiverAccountId";
                var queryObject = new
                {
                    sum = transaction.Sum,
                    senderAccountId = transaction.SenderAccountId,
                    receiverAccountId = transaction.ReceiverAccountId
                };

                var result = await _connection.QueryFirstOrDefaultAsync<TransactionEntity>(sql, queryObject, transaction: transactionSql);
                transactionSql.Commit();
                return result;
            }
        }
    }

    public async Task<IEnumerable<TransactionEntity?>> GetUserTransactionsAsync(Guid id)
    {
        string sql = "SELECT DISTINCT transactions.id AS Id, " +
            "transactions.transaction_type_id AS TransactionTypeId, " +
            "transactions.sum AS Sum, " +
            "transactions.sender_acc_id AS SenderAccountId, " +
            "transactions.receiver_acc_id AS ReceiverAccountId " +
            "FROM transactions " +
            "JOIN accounts AS sender_accounts ON transactions.sender_acc_id = sender_accounts.id " +
            "JOIN accounts AS receiver_accounts ON transactions.receiver_acc_id = receiver_accounts.id " +
            "JOIN users ON (sender_accounts.user_id = users.id OR receiver_accounts.user_id = users.id) " +
            "WHERE users.id = @Id AND transactions.sender_acc_id <> transactions.receiver_acc_id;";
        var queryObject = new
        {
            Id = id
        };
        var result = await _connection.QueryAsync<TransactionEntity?>(sql, queryObject);
        return result;
    }
}
