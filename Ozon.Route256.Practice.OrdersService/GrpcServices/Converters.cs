using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.OrdersService.DataAccess;

namespace Ozon.Route256.Practice.OrdersService.GrpcServices
{
    public class Converters
    {
        public static DataAccess.OrderType ConvertOrderType(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.Web => DataAccess.OrderType.Web,
                OrderType.Api => DataAccess.OrderType.Api,
                OrderType.Mobile => DataAccess.OrderType.Mobile,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderType ConvertOrderType(DataAccess.OrderType orderType)
        {
            return orderType switch
            {
                DataAccess.OrderType.Web => OrderType.Web,
                DataAccess.OrderType.Api => OrderType.Api,
                DataAccess.OrderType.Mobile => OrderType.Mobile,
                _ => throw new NotImplementedException(),
            };
        }

        public static DataAccess.PaginationParameters ConvertPaginationParameters(PaginationParameters paginationParameters)
        {
            return new DataAccess.PaginationParameters(paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public static DataAccess.SortOrder ConvertSortOrder(SortOrder sortOrder)
        {
            return sortOrder switch
            {
                SortOrder.Asc => DataAccess.SortOrder.ASC,
                SortOrder.Desc => DataAccess.SortOrder.DESC,
                _ => throw new NotImplementedException(),
            };
        }

        public static DataAccess.OrderState ConvertOrderState(OrderState orderState)
        {
            return orderState switch
            {
                OrderState.Created => DataAccess.OrderState.Created,
                OrderState.Delivered => DataAccess.OrderState.Delivered,
                OrderState.SentToCustomer => DataAccess.OrderState.SentToCustomer,
                OrderState.Cancelled => DataAccess.OrderState.Cancelled,
                OrderState.Lost => DataAccess.OrderState.Lost,
                _ => throw new NotImplementedException(),
            };
        }


        public static OrderState ConvertOrderState(DataAccess.OrderState orderState)
        {
            return orderState switch
            {
                DataAccess.OrderState.Created => OrderState.Created,
                DataAccess.OrderState.Delivered => OrderState.Delivered,
                DataAccess.OrderState.SentToCustomer => OrderState.SentToCustomer,
                DataAccess.OrderState.Cancelled => OrderState.Cancelled,
                DataAccess.OrderState.Lost => OrderState.Lost,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderItem ConvertOrderEntity(OrderEntity orderEntity)
        {
            return new()
            {
                Address = orderEntity.Address,
                ItemsCount = orderEntity.ItemsCount,
                OrderDate = Timestamp.FromDateTime(orderEntity.OrderDate),
                OrderId = orderEntity.OrderId,
                OrderType = ConvertOrderType(orderEntity.OrderType),
                Phone = orderEntity.Phone,
                Region = orderEntity.Region,
                State = ConvertOrderState(orderEntity.State),
                TotalPrice = orderEntity.TotalPrice,
                TotalWeight = orderEntity.TotalWeight
            };
        }

        public static RegionOrderItem ConvertRegionOrderItem(OrderByRegionEntity orderByRegionEntity)
        {
            return new()
            {
                ClientsCount = orderByRegionEntity.ClientsCount,
                OrdersCount = orderByRegionEntity.OrdersCount,
                Region = orderByRegionEntity.Region,
                TotalPrice = orderByRegionEntity.TotalPrice,
                TotalWeight = orderByRegionEntity.TotalWeight
            };
        }
    }
}
