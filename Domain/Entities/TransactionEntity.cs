using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public int TransationTypeId { get; set; }
    public decimal Sum { get; set; }
    public Guid SenderAccountId { get; set; }
    public Guid ReceiverAccountId { get; set; }
}
