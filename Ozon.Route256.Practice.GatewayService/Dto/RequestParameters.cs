using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.GatewayService
{
    public record GetOrdersRequestParametersDto
    {
        [Required]
        public List<string> Regions { get; set; }
        public OrderType OrderType { get; set; }
        [Required]
        public PaginationParametersDto PaginationParameters { get; set; }
        public SortOrder? SortOrder { get; set; }
        public List<string>? SortingFields { get; set; }

    }

    public record PaginationParametersDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }

    public class OrderRequestValidator : AbstractValidator<GetOrdersRequestParametersDto>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.Regions).Must(x => x != null && x.Any() && x.All(item => !String.IsNullOrEmpty(item)));
        }
    }
}
