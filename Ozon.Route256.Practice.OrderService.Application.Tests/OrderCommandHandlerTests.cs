using Moq;
using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Application.Metrics;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Tests
{
    public class OrderCommandHandlerTests
    {
        [Fact]
        public async Task CreateOrder_ShouldWriteMetrics()
        {
            var metricsMock = new Mock<IOrderMetrics>();

            var handler = new CreateOrderCommandHandler(
                unitOfWork: new Mock<IUnitOfWork>().Object,
                metrics: metricsMock.Object);

            await handler.Handle(new CreateOrderCommand(
                OrderAggregate.CreateInstance(Order.CreateInstance(0, 0, 0,0, OrderType.Api, DateTime.UtcNow, "", OrderState.Created, 0),
                 Customer.CreateInstance(0, "", "", "", ""))),
                It.IsAny<CancellationToken>());

            metricsMock.Verify(mock => mock.OrderCreated(OrderType.Api), Times.Once());
        }

        [Fact]
        public async Task CancelOrder_ShouldWriteMetrics()
        {
            var metricsMock = new Mock<IOrderMetrics>();

            var handler = new CancelOrderCommandHandler(
                unitOfWork: new Mock<IUnitOfWork>().Object,
                metrics: metricsMock.Object);

            await handler.Handle(new CancelOrderCommand(It.IsAny<long>()), It.IsAny<CancellationToken>());

            metricsMock.Verify(mock => mock.OrderCanceled(), Times.Once());
        }
    }
}
