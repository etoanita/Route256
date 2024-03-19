using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.GatewayService
{
    public record GetOrdersRequestParametersDto
    {
        public List<string> Regions { get; set; }
        public OrderType OrderType { get; set; }
        public PaginationParametersDto PaginationParameters { get; set; }
        public SortOrder? SortOrder { get; set; }
        public List<string>? SortingFields { get; set; }

    }

    public record PaginationParametersDto
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
