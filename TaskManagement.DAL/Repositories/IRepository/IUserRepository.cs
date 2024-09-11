namespace TaskManagement.DAL.Repositories.IRepository;

using TaskManagement.DAL.Models;

internal interface IUserRepository
{
    public Task<bool> TryLoginAsync(string login, string password);

    public Task<bool> TryRegisterAsync(User user);
}