﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }

    public enum SortOrder
    {
        ASC = 0, DESC = 1
    }
}
