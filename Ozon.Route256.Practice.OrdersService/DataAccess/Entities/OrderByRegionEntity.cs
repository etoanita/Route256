using System;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Entities
{
    public record OrderByRegionEntity(
        string Region,
        int OrdersCount,
        int TotalPrice,
        int TotalWeight,
        int ClientsCount    
    );
}
