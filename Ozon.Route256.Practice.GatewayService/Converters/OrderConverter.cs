namespace Ozon.Route256.Practice.GatewayService.Converters
{
    public static class OrderConverter
    {
        public static GetOrdersListRequest ConvertGetOrdersRequestParameters(GetOrdersRequestParametersDto requestParameters)
        {
            GetOrdersListRequest result = new();
            result.Regions.AddRange(requestParameters.Regions);
            result.OrderType = requestParameters.OrderType;
            result.PaginationParameters = ConvertPaginationParameters(requestParameters.PaginationParameters);
            result.SortingOrder = requestParameters.SortOrder.HasValue ? (Practice.SortOrder)(requestParameters.SortOrder) : Practice.SortOrder.Asc;
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
            return new
            (
                order.OrderId,
                order.ItemsCount,
                order.TotalPrice,
                order.TotalWeight,
                order.OrderType,
                order.OrderDate.ToDateTime(),
                order.Region,
                order.State,
                order.CustomerName,
                order.CustomerSurname,
                order.Address,
                order.Phone
            );
        }
        public static RegionOrderDto ConvertRegionOrderDto(RegionOrderItem orderItem)
        {
            return new
            (
                orderItem.Region,
                orderItem.OrdersCount,
                orderItem.TotalPrice,
                orderItem.TotalWeight,
                orderItem.ClientsCount
            );
        }
    }
}
