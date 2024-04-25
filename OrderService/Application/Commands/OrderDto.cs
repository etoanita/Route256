using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Commands;

public record OrderDto(
    long Id,
    int ItemsCount,
    double TotalPrice,
    long TotalWeight,
    OrderType OrderType,
    DateTime OrderDate,
    string RegionName,
    OrderState State,
    int CustomerId,
    string CustomerName,
    string CustomerSurname,
    string Address,
    string Phone
);
