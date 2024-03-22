using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.OrdersService.DataAccess.Entities;

namespace Ozon.Route256.Practice.OrdersService.GrpcServices
{
    public class Converters
    {
        public static DataAccess.Entities.OrderType ConvertOrderType(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.Web => DataAccess.Entities.OrderType.Web,
                OrderType.Api => DataAccess.Entities.OrderType.Api,
                OrderType.Mobile => DataAccess.Entities.OrderType.Mobile,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderType ConvertOrderType(DataAccess.Entities.OrderType orderType)
        {
            return orderType switch
            {
                DataAccess.Entities.OrderType.Web => OrderType.Web,
                DataAccess.Entities.OrderType.Api => OrderType.Api,
                DataAccess.Entities.OrderType.Mobile => OrderType.Mobile,
                _ => throw new NotImplementedException(),
            };
        }

        public static DataAccess.Entities.PaginationParameters ConvertPaginationParameters(PaginationParameters paginationParameters)
        {
            return new DataAccess.Entities.PaginationParameters(paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public static DataAccess.Entities.SortOrder ConvertSortOrder(SortOrder sortOrder)
        {
            return sortOrder switch
            {
                SortOrder.Asc => DataAccess.Entities.SortOrder.ASC,
                SortOrder.Desc => DataAccess.Entities.SortOrder.DESC,
                _ => throw new NotImplementedException(),
            };
        }

        public static DataAccess.Entities.OrderState ConvertOrderState(OrderState orderState)
        {
            return orderState switch
            {
                OrderState.Created => DataAccess.Entities.OrderState.Created,
                OrderState.Delivered => DataAccess.Entities.OrderState.Delivered,
                OrderState.SentToCustomer => DataAccess.Entities.OrderState.SentToCustomer,
                OrderState.Cancelled => DataAccess.Entities.OrderState.Cancelled,
                OrderState.Lost => DataAccess.Entities.OrderState.Lost,
                _ => throw new NotImplementedException(),
            };
        }


        public static OrderState ConvertOrderState(DataAccess.Entities.OrderState orderState)
        {
            return orderState switch
            {
                DataAccess.Entities.OrderState.Created => OrderState.Created,
                DataAccess.Entities.OrderState.Delivered => OrderState.Delivered,
                DataAccess.Entities.OrderState.SentToCustomer => OrderState.SentToCustomer,
                DataAccess.Entities.OrderState.Cancelled => OrderState.Cancelled,
                DataAccess.Entities. OrderState.Lost => OrderState.Lost,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderItem ConvertOrderEntity(OrderEntity orderEntity)
        {
            return new()
            {
                Address = orderEntity.Address,
                CustomerName = orderEntity.CustomerName,
                CustomerSurname = orderEntity.CustomerSurname,
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
