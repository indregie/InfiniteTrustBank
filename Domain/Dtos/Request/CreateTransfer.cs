namespace Domain.Dtos.Request;

public class CreateTransfer
{
    public decimal Sum { get; set; }
    public Guid SenderAccountId { get; set; }
    public Guid ReceiverAccountId { get; set; }
}
