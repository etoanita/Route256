using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.GrpcServices
{
    public class Converters
    {
        public static OrderService.Domain.OrderType ConvertOrderType(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.Web => OrderService.Domain.OrderType.Web,
                OrderType.Api => OrderService.Domain.OrderType.Api,
                OrderType.Mobile => OrderService.Domain.OrderType.Mobile,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderType ConvertOrderType(OrderService.Domain.OrderType orderType)
        {
            return orderType switch
            {
                OrderService.Domain.OrderType.Web => OrderType.Web,
                OrderService.Domain.OrderType.Api => OrderType.Api,
                OrderService.Domain.OrderType.Mobile => OrderType.Mobile,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderService.Domain.PaginationParameters ConvertPaginationParameters(PaginationParameters paginationParameters)
        {
            return new OrderService.Domain.PaginationParameters(paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public static OrderService.Domain.SortOrder ConvertSortOrder(SortOrder sortOrder)
        {
            return sortOrder switch
            {
                SortOrder.Asc => OrderService.Domain.SortOrder.ASC,
                SortOrder.Desc => OrderService.Domain.SortOrder.DESC,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderService.Domain.OrderState ConvertOrderState(OrderState orderState)
        {
            return orderState switch
            {
                OrderState.Created => OrderService.Domain.OrderState.Created,
                OrderState.Delivered => OrderService.Domain.OrderState.Delivered,
                OrderState.SentToCustomer => OrderService.Domain.OrderState.SentToCustomer,
                OrderState.Cancelled => OrderService.Domain.OrderState.Cancelled,
                OrderState.Lost => OrderService.Domain.OrderState.Lost,
                _ => throw new NotImplementedException(),
            };
        }


        public static OrderState ConvertOrderState(OrderService.Domain.OrderState orderState)
        {
            return orderState switch
            {
                OrderService.Domain.OrderState.Created => OrderState.Created,
                OrderService.Domain.OrderState.Delivered => OrderState.Delivered,
                OrderService.Domain.OrderState.SentToCustomer => OrderState.SentToCustomer,
                OrderService.Domain.OrderState.Cancelled => OrderState.Cancelled,
                OrderService.Domain.OrderState.Lost => OrderState.Lost,
                _ => throw new NotImplementedException(),
            };
        }

        public static OrderItem ConvertOrderEntity(OrderService.Domain.Order orderEntity)
        {
            return new()
            {
                //Address = orderEntity.Address,
                ItemsCount = orderEntity.ItemsCount,
                OrderDate = Timestamp.FromDateTimeOffset(orderEntity.OrderDate),
                OrderId = orderEntity.Id,
                OrderType = ConvertOrderType(orderEntity.OrderType),
                //Phone = orderEntity.Phone,
                Region = orderEntity.Region,
                State = ConvertOrderState(orderEntity.State),
                TotalPrice = orderEntity.TotalPrice,
                TotalWeight = orderEntity.TotalWeight
            };
        }

        public static RegionOrderItem ConvertRegionOrderItem(OrderByRegion orderByRegionEntity)
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

        public static OrderAggregate CreateOrderEntity(NewOrder order, Customer customer, DateTime orderDate)
        {
            return OrderAggregate.CreateInstance(
                OrderService.Domain.Order.CreateInstance(
                order.Id,
                order.Goods.Count,
                order.Goods.Sum(x=>x.Price),
                order.Goods.Sum(x => x.Weight),
                (OrderService.Domain.OrderType)(order.Source),
                orderDate,
                order.Customer.Address.Region,
                OrderService.Domain.OrderState.Created,
                order.Customer.Id), OrderService.Domain.Customer.CreateInstance(order.Customer.Id,
                customer.FirstName,
                customer.LastName,
                order.Customer.Address.ToString(),
                customer.MobileNumber
            ));
        }
    }
}
