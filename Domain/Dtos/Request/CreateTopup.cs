namespace Domain.Dtos.Request;

public class CreateTopup
{
    public decimal Sum { get; set; }
    public Guid AccountId { get; set; }
}
