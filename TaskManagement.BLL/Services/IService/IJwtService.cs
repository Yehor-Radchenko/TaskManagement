namespace TaskManagement.BLL.Services.IService;

using TaskManagement.DAL.Models;

public interface IJwtService
{
    public string GenerateToken(User user);
}
