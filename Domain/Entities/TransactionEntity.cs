﻿namespace Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public int TransactionTypeId { get; set; }
    public decimal Sum { get; set; }
    public Guid SenderAccountId { get; set; }
    public Guid ReceiverAccountId { get; set; }
}
