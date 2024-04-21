using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Dal.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Database.Mappers;

internal interface IDataWriteMapper
{
    OrderDal ToOrderDao(Order order);
    OrderService.Dal.Models.OrderType ToOrderTypeDal(Domain.OrderType orderType);
    OrderService.Dal.Models.OrderState ToOrderStateDal(Domain.OrderState orderState);
    OrderService.Dal.Models.SortOrder ToSortOrderDal(Domain.SortOrder orderState);

}
