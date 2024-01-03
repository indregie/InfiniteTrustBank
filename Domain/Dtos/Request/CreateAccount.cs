namespace Domain.Dtos.Request;

public class CreateAccount
{
    public int TypeId { get; set; }
    public Guid UserId { get; set; }
}
