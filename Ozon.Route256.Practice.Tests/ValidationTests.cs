using Microsoft.AspNetCore.Http;
using Ozon.Route256.Practice.GatewayService;
using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.Tests
{
    public class ValidationTests
    {
        [Fact]
        public void TestNegativePageNumber()
        {
            var obj = new PaginationParametersDto { PageNumber = -1, PageSize = 4 };
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, context, results, true);
            Assert.False(valid);
        }

        [Fact]
        public void TestNegativePageSize()
        {
            var obj = new PaginationParametersDto { PageNumber = 1, PageSize = -4};
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, context, results, true);
            Assert.False(valid);
        }

        [Fact]
        public void TestValidPaginationParametersDtoModel()
        {
            var obj = new PaginationParametersDto { PageNumber = 1, PageSize = 7 };
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, context, results, true);
            Assert.True(valid);
        }

        [Fact]
        public void TestEmptyGetOrdersRequestParametersDto()
        {
            var testRequest = new GetOrdersRequestParametersDto();
            var context = new ValidationContext(testRequest);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(testRequest, context, results, true);
            Assert.False(valid);
        }


        [Fact]
        public void TestEmptyStringInRegions()
        {
            var validator = new OrderRequestValidator();
            GetOrdersRequestParametersDto testRequest = new()
            {
                Regions = new List<string>() { "", "abc" },
                PaginationParameters = new PaginationParametersDto { PageNumber = 1, PageSize = 7 }
            };
            var result = validator.Validate(testRequest);
            Assert.Contains(result.Errors, o => o.PropertyName == "Regions");
        }

        [Fact]
        public void TestEmptyRegion()
        {
            GetOrdersRequestParametersDto testRequest = new()
            {
                Regions = new List<string>() { "asd", "abc" },
            };
            var context = new ValidationContext(testRequest);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(testRequest, context, results, true);
            Assert.False(valid);
        }

        [Fact]
        public void TestEmptyPaginationParameters()
        {
            GetOrdersRequestParametersDto testRequest = new()
            {
                PaginationParameters = new PaginationParametersDto { PageNumber = 1, PageSize = 7 }
            };
            var context = new ValidationContext(testRequest);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(testRequest, context, results, true);
            Assert.False(valid);
        }


        [Fact]
        public void TestGetOrdersRequestParametersDtoOk()
        {
            var validator = new OrderRequestValidator();
            GetOrdersRequestParametersDto testRequest = new()
            {
                Regions = new List<string>() { "asd", "abc" },
                PaginationParameters = new PaginationParametersDto { PageNumber = 1, PageSize = 7 }
            };
            var result = validator.Validate(testRequest);
            Assert.Empty(result.Errors);
            var context = new ValidationContext(testRequest);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(testRequest, context, results, true);
            Assert.True(valid);
        }
    }
}