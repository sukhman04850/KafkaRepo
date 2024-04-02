using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using OrderService;
using OrderService.Business_Layer;
using OrderService.Controllers;
using OrderService.Interfaces;
using OrderService.Model;
using OrderService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService_UnitTests
{
    public class UnitTest_OrderRepository
    {
        [Fact]
        public async Task GetOrderById_ShouldReturnOrderList()
        {
            // Arrange
            var orderId = 1; // Replace with your test order ID
            var ordersData = new List<Orders>
        {
            new Orders { OrderID = 1, ProductId = 1, Price = 100, Quantity = 5 },
            new Orders { OrderID = 2, ProductId = 2, Price = 200, Quantity = 4 },
        };

            var orderRepositoryMock = Substitute.For<IOrderInterface>();
            orderRepositoryMock.GetOrderById(orderId).Returns(Task.FromResult(ordersData));

            var orderService = new OrderRepositoryBL(orderRepositoryMock);

            // Act
            var result = await orderService.GetOrderById(orderId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(ordersData); // Use BeEquivalentTo from Fluent Assertions for list comparison


        }
        [Fact]
        public async Task AddOrder_ShouldReturnAddedOrder()
        {
            // Arrange
            var orderRequest = new Orders { OrderID = 1, ProductId = 1, Price = 100, Quantity = 5 };
            var addedOrder = new Orders { OrderID = 1, ProductId = 1, Price = 100, Quantity = 5 }; // Mock the expected added order
            var orderRepositoryMock = Substitute.For<IOrderInterface>();
            orderRepositoryMock.AddOrder(Arg.Any<Orders>()).Returns(Task.FromResult(addedOrder));

            var orderService = new OrderRepositoryBL(orderRepositoryMock);

            // Act
            var result = await orderService.AddOrder(orderRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(addedOrder);
        }
        [Fact]
        public async Task GetAllOrders_ShouldReturnListOfOrders()
        {
            // Arrange
            var expectedOrders = new List<Orders>
        {
            new Orders { OrderID = 1, ProductId = 1, Price = 100, Quantity = 5 },
            new Orders { OrderID = 2, ProductId = 2, Price = 200, Quantity = 4 },
            // Add more orders as needed for your test case
        };

            var orderRepositoryMock = Substitute.For<IOrderInterface>();
            orderRepositoryMock.GetAllOrders().Returns(Task.FromResult(expectedOrders));

            var orderService = new OrderRepositoryBL(orderRepositoryMock);

            // Act
            var result = await orderService.GetAllOrders();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedOrders);
        }
        [Fact]
        public async Task UpdateOrder_ShouldReturnUpdatedOrder()
        {
            // Arrange
            var orderUpdateRequest = new Orders { OrderID = 1, ProductId = 1, Price = 100, Quantity = 5 };
            var updatedOrder = new Orders { OrderID = 1, ProductId = 1, Price = 150, Quantity = 8 }; // Mock the expected updated order
            var orderRepositoryMock = Substitute.For<IOrderInterface>();
            orderRepositoryMock.UpdateOrder(Arg.Any<Orders>()).Returns(Task.FromResult(updatedOrder));

            var orderService = new OrderRepositoryBL(orderRepositoryMock);

            // Act
            var result = await orderService.UpdateOrder(orderUpdateRequest);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedOrder);
        }
        }
    }
