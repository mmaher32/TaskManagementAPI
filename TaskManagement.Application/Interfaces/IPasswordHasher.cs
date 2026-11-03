namespace TaskManagement.Application.Interfaces;

public interface IPasswordHasher
{
    string Hash(string password);
    public bool verify(string password, string passwordHash);
}
