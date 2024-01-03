namespace Domain.Entities;

public class AccountEntity
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; } = 0;
    public int TypeId { get; set; }
    public Guid UserId { get; set; }
    public bool IsDeleted {  get; set; }
}
