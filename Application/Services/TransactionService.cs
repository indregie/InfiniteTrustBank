using Domain.Dtos.Request;
using Domain.Dtos.Response;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;

namespace Application.Services;

public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly AccountService _accountService;

    public TransactionService(ITransactionRepository transactionRepository, AccountService accountService)
    {
        _transactionRepository = transactionRepository;
        _accountService = accountService;
    }

    public async Task<Transaction> CreateTransfer(CreateTransfer transaction)
    {
        if (transaction.Sum < 0)
        {
            throw new NegativeAmountException();
        }

        var senderAccount = await _accountService.GetById(transaction.SenderAccountId);
        var receiverAccount = await _accountService.GetById(transaction.ReceiverAccountId);
        if (senderAccount == null || receiverAccount == null)
        {
            throw new AccountNotFoundException();
        }

        if (senderAccount.Balance < transaction.Sum + 1)
        {
            throw new InsufficientFundsException();
        }

        TransactionEntity transEn = new TransactionEntity()
        {
            TransactionTypeId = 3,
            Sum = transaction.Sum,
            SenderAccountId = transaction.SenderAccountId,
            ReceiverAccountId = transaction.ReceiverAccountId
        };

        TransactionEntity result = await _transactionRepository.CreateTransferAsync(transEn) ?? throw new Exception();
        Transaction response = new Transaction()
        {
            Id = result.Id,
            TransactionTypeId = 3,
            Sum = result.Sum,
            SenderAccountId = result.SenderAccountId,
            ReceiverAccountId = result.ReceiverAccountId
        };

        return response;
    }

    public async Task<Transaction> CreateTopup(CreateTopup transaction)
    {
        if (transaction.Sum < 0)
        {
            throw new NegativeAmountException();
        }

        var senderAccount = await _accountService.GetById(transaction.AccountId);
        if (senderAccount == null)
        {
            throw new AccountNotFoundException();
        }

        if (senderAccount.Balance < transaction.Sum + 1)
        {
            throw new InsufficientFundsException();
        }

        TransactionEntity transEn = new TransactionEntity()
        {
            TransactionTypeId = 2,
            Sum = transaction.Sum,
            SenderAccountId = transaction.AccountId,
            ReceiverAccountId = transaction.AccountId
        };

        TransactionEntity result = await _transactionRepository.CreateTopupAsync(transEn) ?? throw new Exception();
        Transaction response = new Transaction()
        {
            Id = result.Id,
            TransactionTypeId = 2,
            Sum = result.Sum,
            SenderAccountId = result.SenderAccountId,
            ReceiverAccountId = result.ReceiverAccountId
        };

        return response;
    }

    public async Task<IEnumerable<Transaction>> GetByUser(Guid id)
    {
        IEnumerable<TransactionEntity> userTransactions = await _transactionRepository.GetUserTransactionsAsync(id);
        IEnumerable<Transaction> responseTransactions = userTransactions
            .Select(transaction => new Transaction
            {
                Id = transaction.Id,
                TransactionTypeId = transaction.TransactionTypeId,
                Sum = transaction.Sum,
                SenderAccountId = transaction.SenderAccountId,
                ReceiverAccountId = transaction.ReceiverAccountId
            });
        return responseTransactions;
    }
}
