using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity?> CreateTransferAsync(TransactionEntity transaction);
        Task<TransactionEntity?> CreateTopupAsync(TransactionEntity transaction);
        Task<IEnumerable<TransactionEntity?>> GetUserTransactionsAsync(Guid id);
    }
}