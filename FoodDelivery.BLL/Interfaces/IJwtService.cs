namespace FoodDelivery.BLL.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
        Guid? ValidateToken(string token);
    }
}