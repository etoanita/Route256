using Ozon.Route256.Practice.GatewayService.Dto;
using System;
using static Ozon.Route256.Practice.GatewayService.Controllers.OrdersController;

namespace Ozon.Route256.Practice.GatewayService.Converters
{
    public static class OrderConverter
    {
        public static GetOrdersListRequest ConvertGetOrdersRequestParameters(GetOrdersRequestParametersDto requestParameters)
        {
            GetOrdersListRequest result = new GetOrdersListRequest();
            result.Regions.AddRange(requestParameters.Regions);
            result.OrderType = requestParameters.OrderType;
            result.PaginationParameters = ConvertPaginationParameters(requestParameters.PaginationParameters);
            result.SortingOrder = requestParameters.SortOrder.HasValue ? (SortOrder)(requestParameters.SortOrder) : SortOrder.Asc;
            if (requestParameters.SortingFields != null)
                result.SortingField.AddRange(requestParameters.SortingFields);
            return result;
        }

        public static PaginationParameters ConvertPaginationParameters(PaginationParametersDto paginationParameters)
        {
            return new PaginationParameters
            {
                PageNumber = paginationParameters.PageNumber,
                PageSize = paginationParameters.PageSize
            };
        }

        public static OrderDto ConvertOrderDto(OrderItem order)
        {
            return new()
            {
                OrderId = order.OrderId,
                ItemsCount = order.ItemsCount,
                TotalPrice = order.TotalPrice,
                TotalWeight = order.TotalWeight,
                OrderType = order.OrderType,
                OrderDate = order.OrderDate.ToDateTime(),
                Region = order.Region,
                State = order.State,
                CustomerName = order.CustomerName,
                CustomerSurname = order.CustomerSurname,
                Address = order.Address,
                Phone = order.Phone
            };
        }
        public static RegionOrderDto ConvertRegionOrderDto(RegionOrderItem orderItem)
        {
            return new RegionOrderDto
            {
                Region = orderItem.Region,
                OrdersCount = orderItem.OrdersCount,
                TotalPrice = orderItem.TotalPrice,
                TotalWeight = orderItem.TotalWeight,
                ClientsCount = orderItem.ClientsCount
            };
        }
    }
}
