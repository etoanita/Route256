using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.GatewayService
{
    public record GetOrdersRequestParametersDto
    {
        [Required]
        public List<string> Regions { get; init; }
        public OrderType OrderType { get; init; }
        [Required]
        public PaginationParametersDto PaginationParameters { get; init; }
        public SortOrder? SortOrder { get; init; }
        public List<string>? SortingFields { get; init; }

    }

    public record PaginationParametersDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PageSize { get; init; }
    }

    public class OrderRequestValidator : AbstractValidator<GetOrdersRequestParametersDto>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.Regions).Must(x => x != null && x.Any() && x.All(item => !String.IsNullOrEmpty(item)));
        }
    }
}
