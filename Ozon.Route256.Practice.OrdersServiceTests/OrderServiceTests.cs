using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Moq;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.DataAccess;
using Ozon.Route256.Practice.OrderService.Exceptions;
using static Ozon.Route256.Practice.LogisticsSimulator.Grpc.LogisticsSimulatorService;

namespace Ozon.Route256.Practice.OrderService.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IRegionService> _regionsRepositoryMock;
        private readonly Mock<IOrderService> _ordersRepositoryMock;
        private readonly Mock<LogisticsSimulatorServiceClient> _logisticSimulatorMock;
        private Infrastructure.GrpcServices.OrdersService _ordersService;
        public OrderServiceTests() 
        {
            _regionsRepositoryMock = new Mock<IRegionService>();
            _ordersRepositoryMock = new Mock<IOrderService>();
            _logisticSimulatorMock = new Mock<LogisticsSimulatorServiceClient>();
        }

        private void SetupLogisticSimulator(CancelResult result)
        {
            _logisticSimulatorMock.Setup(repo => repo.OrderCancelAsync(It.IsAny<Order>(), It.IsAny<Metadata>(),
            It.IsAny<DateTime?>(), It.IsAny<CancellationToken>())).Returns(
            new AsyncUnaryCall<CancelResult>(Task.FromResult(result), default, default, default, default, default));
        }

        private void SetupOrderService()
        {
            _ordersService = new Infrastructure.GrpcServices.OrdersService(_ordersRepositoryMock.Object, 
                _regionsRepositoryMock.Object, _logisticSimulatorMock.Object);
        }


        [Fact]
        public async void CancelOrderLogisticsCannotCancel()
        {
            var result = new CancelResult
            {
                Error = "error message",
                Success = false
            };
            SetupLogisticSimulator(result);
            SetupOrderService();
            var res = await _ordersService.CancelOrder(new CancelOrderRequest(), default);
            Assert.False(res.Success);
            Assert.Equal("error message", res.Message);
        }

        [Fact]
        public async void CancelOrderOrderNotFound()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _ordersRepositoryMock.Setup(client => client.CancelOrder(0, CancellationToken.None))
                .ThrowsAsync(new NotFoundException("not found"));
            SetupOrderService();
            Task result() => _ordersService.CancelOrder(new CancelOrderRequest(), new ServerCallContextMock());
            RpcException res = await Assert.ThrowsAsync<RpcException>(result);
            Assert.Equal(StatusCode.NotFound, res.Status.StatusCode);
            Assert.Contains("not found", res.Message);
        }

        [Fact]
        public async void CancelOrderOrderInappropriateState()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _ordersRepositoryMock.Setup(client => client.CancelOrder(0, CancellationToken.None))
                .ThrowsAsync(new BadRequestException("invalid"));
            SetupOrderService();
            var result = await _ordersService.CancelOrder(new CancelOrderRequest(), new ServerCallContextMock());
            Assert.False(result.Success);
            Assert.Equal("invalid", result.Message);
        }

        [Fact]
        public async void CancelOrderOk()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _ordersRepositoryMock.Setup(client => client.CancelOrder(0, CancellationToken.None));
            SetupOrderService();
            var result = await _ordersService.CancelOrder(new CancelOrderRequest(), new ServerCallContextMock());
            Assert.True(result.Success);
            Assert.Equal(System.String.Empty, result.Message);
        }

        [Fact]
        public async void GetOrderStateOrderNotFound()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _ordersRepositoryMock.Setup(client => client.GetOrderState(0, CancellationToken.None)).ThrowsAsync(new NotFoundException("not found"));
            SetupOrderService();
            Task result() => _ordersService.GetOrderState(new GetOrderStateRequest(), new ServerCallContextMock());
            RpcException res = await Assert.ThrowsAsync<RpcException>(result);
            Assert.Equal(StatusCode.NotFound, res.Status.StatusCode);
            Assert.Contains("not found", res.Message);
        }

        [Fact]
        public async void GetOrderStateOk()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _ordersRepositoryMock.Setup(client => client.GetOrderState(0, CancellationToken.None))
                .Returns(Task.FromResult(OrderState.SentToCustomer));
            SetupOrderService();
            var state = await _ordersService.GetOrderState(new GetOrderStateRequest(), new ServerCallContextMock());
            Assert.Equal(OrderState.SentToCustomer, state.State);
        }

        [Fact]
        public async void GetRegionsList()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _regionsRepositoryMock.Setup(client => client.GetRegionsList(CancellationToken.None))
                .Returns(Task.FromResult(new List<string> { "moscow" }.AsReadOnly() as IReadOnlyCollection<string>));
            SetupOrderService();
            var regions = await _ordersService.GetRegionsList(new GetRegionsListRequest(), new ServerCallContextMock());
            Assert.Single(regions.Regions);
            Assert.Equal("moscow", regions.Regions[0]);
        }

        [Fact]
        public async void GetOrdersListRegionNotFound()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _regionsRepositoryMock.Setup(client => client.FindNotPresented(new List<string> { "perm" }, CancellationToken.None))
                .Returns(Task.FromResult(new List<string> { "perm" }.AsReadOnly() as IReadOnlyCollection<string>));
            SetupOrderService();
            var request = new GetOrdersListRequest();
            request.Regions.Add(new List<string> { "perm" });
            Task result() => _ordersService.GetOrdersList(request, new ServerCallContextMock());
            RpcException res = await Assert.ThrowsAsync<RpcException>(result);
            Assert.Equal(StatusCode.InvalidArgument, res.Status.StatusCode);
            Assert.Contains("perm", res.Message);
        }

        [Fact]
        public async void GetOrdersListOk()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _regionsRepositoryMock.Setup(client => client.FindNotPresented(new List<string> { }, CancellationToken.None))
                .Returns(Task.FromResult(new List<string> { }.AsReadOnly() as IReadOnlyCollection<string>));
            var getOrderRequest = new GetOrdersListRequest
            {
                OrderType = It.IsAny<OrderType>(),
                PaginationParameters = new PaginationParameters(),
                SortingOrder = It.IsAny<SortOrder>()
            };
            getOrderRequest.Regions.AddRange(new List<string>());
            getOrderRequest.SortingField.AddRange(new List<string>());
            _ordersRepositoryMock.Setup(client => client.GetOrders(getOrderRequest, CancellationToken.None)).ReturnsAsync(new List<OrderItem> { });
            SetupOrderService();
                await _ordersService.GetOrdersList(new GetOrdersListRequest() { PaginationParameters = new PaginationParameters()}, new ServerCallContextMock());
        }

        [Fact]
        public async void GetOrdersByRegionsRegionNotFound()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _regionsRepositoryMock.Setup(client => client.FindNotPresented(new List<string> { "perm" }, CancellationToken.None))
                .Returns(Task.FromResult(new List<string> { "perm" }.AsReadOnly() as IReadOnlyCollection<string>));
            SetupOrderService();
            var request = new GetOrdersByRegionsRequest();
            request.Regions.Add(new List<string> { "perm" });
            Task result() => _ordersService.GetOrdersByRegions(request, new ServerCallContextMock());
            RpcException res = await Assert.ThrowsAsync<RpcException>(result);
            Assert.Equal(StatusCode.InvalidArgument, res.Status.StatusCode);
            Assert.Contains("perm", res.Message);
        }

        [Fact]
        public async void GetOrdersByRegionsOk()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            _regionsRepositoryMock.Setup(client => client.FindNotPresented(new List<string> { }, CancellationToken.None))
                .Returns(Task.FromResult(new List<string> { }.AsReadOnly() as IReadOnlyCollection<string>));
            var request = new GetOrdersByRegionsRequest();
            request.Regions.Add(new List<string>());
            request.StartDate = Timestamp.FromDateTime(new DateTime().ToUniversalTime());
            _ordersRepositoryMock.Setup(client => client.GetOrdersByRegions(request, CancellationToken.None)).ReturnsAsync(new List<RegionOrderItem>());
            SetupOrderService();
            await _ordersService.GetOrdersByRegions(new GetOrdersByRegionsRequest() { StartDate = Timestamp.FromDateTimeOffset(new DateTime().ToUniversalTime())}, new ServerCallContextMock());
        }

        [Fact]
        public async void GetOrdersByClientIdOk()
        {
            SetupLogisticSimulator(new CancelResult { Success = true });
            var request = new GetOrdersByClientIdRequest
            {
                ClientId = 0,
                PaginationParameters = new PaginationParameters(),
                StartDate = Timestamp.FromDateTimeOffset(It.IsAny<DateTime>().ToUniversalTime())
            };
            _ordersRepositoryMock.Setup(client => client.GetOrdersByClientId(request, CancellationToken.None))
                .ReturnsAsync(new List<OrderItem>());
            SetupOrderService();
            await _ordersService.GetOrdersByClientId(new GetOrdersByClientIdRequest { StartDate = Timestamp.FromDateTimeOffset(new DateTime().ToUniversalTime()),
            PaginationParameters = new PaginationParameters(), ClientId = 0 }, new ServerCallContextMock());
        }
    }
}