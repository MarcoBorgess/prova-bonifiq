using Castle.Core.Resource;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using ProvaPub.Tests;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace CustomerServiceTest
{
    public class Tests
    {
        [Fact]
        public void Can_Purchase()
        {
            CustomerServiceTests customerService = new CustomerServiceTests();
            List<Customer> customers = new List<Customer>();
            List<Order> orders = new List<Order>();
            customers.Add(new Customer
            {
                Id = 1,
                Name = "Test",
                Orders = orders
            });
            int customerId = 1;
            decimal purchaseValue = 89.04m;

            // Act
            var result = customerService.CanPurchase(customerId, purchaseValue, customers, orders);

            // Assert
            result.Result.Should().BeTrue();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(11)]
        public void Non_Registered_Customers_Cannot_Purchase(int customerId)
        {
            // Arrange
            CustomerServiceTests customerService = new CustomerServiceTests();
            List<Customer> customers = new List<Customer>();
            List<Order> orders = new List<Order>();
            decimal purchaseValue = 10;

            // Act
            var result = customerService.CanPurchase(customerId, purchaseValue, customers, orders);

            // Assert
            result.Exception?.Message.Should().Contain($"Customer Id {customerId} does not exists");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-48)]
        public void CustomerId_OutOfRange_Return_Exception_With_Invalid_Field(int customerId)
        {
            // Arrange
            CustomerServiceTests customerService = new CustomerServiceTests();
            List<Customer> customers = new List<Customer>();
            List<Order> orders = new List<Order>();
            decimal purchaseValue = 10;

            // Act
            var result = customerService.CanPurchase(customerId, purchaseValue, customers, orders);

            // Assert
            result.Exception?.Message.Should().Contain("customerId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-48)]
        public void PurchaseValue_OutOfRange_Return_Exception_With_Invalid_Field(decimal purchaseValue)
        {
            // Arrange
            CustomerServiceTests customerService = new CustomerServiceTests();
            List<Customer> customers = new List<Customer>();
            List<Order> orders = new List<Order>();
            int customerId = 1;

            // Act
            var result = customerService.CanPurchase(customerId, purchaseValue, customers, orders);

            // Assert
            result.Exception?.Message.Should().Contain("purchaseValue");
        }

        [Fact]
        public void A_Customer_Can_Purchase_Only_A_Single_Time_Per_Month()
        {
            // Arrange
            CustomerServiceTests customerService = new CustomerServiceTests();
            List<Customer> customers = new List<Customer>();
            List<Order> orders = new List<Order>();
            orders.Add(new Order
            {
                Id = 1,
                Value = 10,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow
            });
            customers.Add(new Customer
            {
                Id = 1,
                Name = "Test",
                Orders = orders
            });
            int customerId = 1;
            decimal purchaseValue = 10;

            // Act
            var result = customerService.CanPurchase(customerId, purchaseValue, customers, orders);

            // Assert
            result.Result.Should().BeFalse();
        }

        [Theory]
        [InlineData(100, true)]
        [InlineData(101, false)]
        [InlineData(100.01, false)]
        [InlineData(1.001, true)]
        public void A_Customer_That_Never_Bought_Before_Can_Make_A_First_Purchase_Of_Maximum_100(decimal purchaseValue, bool expectedValue)
        {
            // Arrange
            CustomerServiceTests customerService = new CustomerServiceTests();
            List<Customer> customers = new List<Customer>();
            List<Order> orders = new List<Order>();
            customers.Add(new Customer
            {
                Id = 1,
                Name = "Test",
                Orders = orders
            });
            int customerId = 1;

            // Act
            var result = customerService.CanPurchase(customerId, purchaseValue, customers, orders);

            // Assert
            result.Result.Should().Be(expectedValue);
        }
    }
}