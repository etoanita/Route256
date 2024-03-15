using Microsoft.AspNetCore.Mvc;

namespace Ozon.Route256.Practice.GatewayService.Dto
{
    public class GetOrdersRequestParametersDto
    {
        public List<string> Regions { get; set; }
        public string OrderType { get; set; }
        public PaginationParametersDto PaginationParameters { get; set; }

        public SortOrder? SortOrder { get; set; }
        public List<string>? SortingFields { get; set; }

    }

    public class PaginationParametersDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public enum SortOrder
    {
        ASC = 0, DESC = 1
    }
}
