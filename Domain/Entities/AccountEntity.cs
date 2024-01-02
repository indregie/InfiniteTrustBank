namespace Domain.Entities;

public class AccountEntity
{
    public Guid Id { get; set; }
    public string AccName { get; set; }
    public decimal Balance { get; set; }
    public Guid TypeId { get; set; }
    public Guid UserId { get; set; }
    public bool IsDeleted {  get; set; }
}
