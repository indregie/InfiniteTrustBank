namespace Domain.Dtos.Response;

public class Account
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; } 
    public int TypeId { get; set; }
    public Guid UserId { get; set; }
}
