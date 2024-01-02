namespace Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public bool IsDeleted { get; set; }
}
