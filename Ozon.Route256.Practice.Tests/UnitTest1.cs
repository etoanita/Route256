using Ozon.Route256.Practice.GatewayService.Dto;
using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var obj = new PaginationParametersDto { PageNumber = -1, PageSize = 4 };
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, context, results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Test2()
        {
            var obj = new PaginationParametersDto { PageNumber = -1, PageSize = 4};
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, context, results, true);
            Assert.False(valid);
        }

        [Fact]
        public void Test3()
        {
            var obj = new PaginationParametersDto { PageNumber = 1, PageSize = 7 };
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(obj, context, results, true);
            Assert.True(valid);
        }
    }
}