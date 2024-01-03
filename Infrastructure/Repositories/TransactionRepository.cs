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
                string sql = "INSERT INTO public.transactions (transaction_type_id, sum, sender_acc_id, receiver_acc_id) " +
                    "VALUES (1, 1, @SenderAccountId, @SenderAccountId) ";
                var queryObject = new
                {
                    SenderAccountId = transaction.SenderAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = "UPDATE public.accounts SET balance=balance-1 WHERE id = @SenderAccountId";
                var queryObject = new
                {
                    SenderAccountId = transaction.SenderAccountId
                };

                var result = await _connection.ExecuteAsync(sql, queryObject, transaction: transactionSql);
            }
            {
                string sql = "INSERT INTO public.transactions (transaction_type_id, sum, sender_acc_id, receiver_acc_id) " +
                "VALUES (3, @Sum, @SenderAccountId, @ReceiverAccountId) " +
                "RETURNING id as Id, transaction_type_id as TransactionTypeId, sum as Sum, " +
                "sender_acc_id as SenderAccountId, receiver_acc_id as ReceiverAccountId";
                var queryObject = new
                {
                    Sum = transaction.Sum,
                    SenderAccountId = transaction.SenderAccountId,
                    ReceiverAccountId = transaction.ReceiverAccountId
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
                "VALUES (2, @Sum, @SenderAccountId, @ReceiverAccountId) " +
                "RETURNING id as Id, transaction_type_id as TransactionTypeId, sum as Sum, " +
                "sender_acc_id as SenderAccountId, receiver_acc_id as ReceiverAccountId";
                var queryObject = new
                {
                    Sum = transaction.Sum,
                    SenderAccountId = transaction.SenderAccountId,
                    ReceiverAccountId = transaction.ReceiverAccountId
                };

                var result = await _connection.QueryFirstOrDefaultAsync<TransactionEntity>(sql, queryObject, transaction: transactionSql);
                transactionSql.Commit();
                return result;
            }
        }
    }
}
