namespace MinimalWebAPI.Models;

public record Product(string Name, string? Description, double UnitPrice, int Quantity)
{
    public double TotalPrice => UnitPrice * Quantity;
}